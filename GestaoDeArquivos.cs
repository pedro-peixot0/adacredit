using static BCrypt.Net.BCrypt;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdaCredit
{
    public partial class Utils
    {
        private static string currrentDir = Directory.GetCurrentDirectory();
        private static CsvHelper.Configuration.CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture){HasHeaderRecord = false};
        private static string filePathUsers = Path.Combine(currrentDir, "data/users.csv");
        // private string[][] LerArquivoComCsvHelper(string nomeArquivo, Func<string[],bool> FilterFunction)
        // {
        //     string filePath = Path.Combine(currrentDir, nomeArquivo);
            
        //     using var reader = new StreamReader(filePath);
        //     using var csv = new CsvParser(reader, csvConfig);
                
        //     if(!csv.Read())
        //         throw new ArgumentException("Found a problem in yout csv file");
            
        //     List<string[]> arrayList = new List<string[]>();
        //     do{
        //         string[] linhaAtual = csv.Record.ToArray();

        //         if (FilterFunction(linhaAtual))
        //             arrayList.Add(linhaAtual);
        //     }while(csv.Read());
  
        //     return arrayList.ToArray();
        // }

        public static void SalvarUsuarioEmArquivo(Usuario user)
        {   
            using (var stream = File.Open(filePathUsers, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, csvConfig))

            if (user is Cliente cliente)
            {
                List<Cliente> records = new List<Cliente>{cliente};
                
                csv.WriteRecords(records);
            }
            else if (user is Funcionario funcionario)
            {
                List<Funcionario> records = new List<Funcionario>{funcionario};
                csv.WriteRecords(records);
            }
        }
        public static Usuario? CarregarUsuarioSalvo(string login, string senha)
        {
            using (var reader = new StreamReader(filePathUsers))
            using (var csv = new CsvReader(reader, csvConfig))            

            while (csv.Read())
            {
                string loginCheck;
                if (
                    csv.TryGetField(7,out loginCheck) &&
                    (login == loginCheck) &&
                    (ChecarSenhaContraHash(senha,csv.GetField(9),csv.GetField(8)))
                )
                    return csv.GetRecord<Cliente>();
                else if (
                    csv.TryGetField(3,out loginCheck) &&
                    (login == loginCheck) &&
                    (ChecarSenhaContraHash(senha,csv.GetField(5),csv.GetField(4)))
                )
                    return csv.GetRecord<Funcionario>();
                else
                    continue;
            }

            return null;
        }
    }
}