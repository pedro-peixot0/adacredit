using static BCrypt.Net.BCrypt;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdaCredit
{
    public partial class Utils
    { 
        private static string  desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static CsvHelper.Configuration.CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture){HasHeaderRecord = false};
        private static string currrentDir = Directory.GetCurrentDirectory();
        private static string filePathUsers = Path.Combine(currrentDir, "data/users.csv");

        public static void ProcessarTransacoes(List<Transacao?> listaTransacoes, DateTime dataTransacoes, string bancoParceiro)
        {
            DateTime dataLimiteGratuidade = new DateTime(2022,11,30);

            foreach (Transacao? transacao in listaTransacoes)
            {
                //pulando transaçoes que não envolvem o banco
                if((transacao.NumeroBancoOrigem != 777 && transacao.NumeroBancoDestino != 777))
                {
                    SalvarTransacao(transacao,"Completed",bancoParceiro,dataTransacoes.ToString("yyyyMMdd"));
                    continue;
                }
                //remover transações inválidas
                if(transacao.TipoTransacao == "TEF" && transacao.NumeroBancoOrigem != transacao.NumeroBancoDestino)
                {
                    SalvarTransacao(transacao,"Failed",bancoParceiro,dataTransacoes.ToString("yyyyMMdd"));
                    continue;
                }

                //adicionar tarifas às transações de crédito do banco e realizar transação
                if  ((transacao.NumeroBancoOrigem == 777) && (transacao.SentidoTransacao == 0))
                {
                    if (dataTransacoes > dataLimiteGratuidade)
                    {
                        switch(transacao.TipoTransacao)
                        {
                            case "TED":
                                transacao.ValorTransacao += 5M; break;
                            case "DOC":
                                transacao.ValorTransacao += transacao.ValorTransacao * 0.01M >= 5 ? transacao.ValorTransacao * 0.01M : 5; break;
                        }
                    }
                }

                int contaAlvo;
                decimal multiplicador;
                //tentar adicionar ou retirar fundos
                if (transacao.SentidoTransacao == 0)
                {
                    contaAlvo = transacao.NumeroContaOrigem;
                    if (transacao.NumeroAgenciaOrigem == 777)
                        multiplicador = -1;
                    else
                        multiplicador = 1;
                }
                else
                {
                    contaAlvo = transacao.NumeroContaDestino;
                    if (transacao.NumeroAgenciaOrigem == 777)
                        multiplicador = 1;
                    else
                        multiplicador = -1;
                }

                if (AtualizarSaldoConta(contaAlvo,transacao.ValorTransacao*multiplicador))
                    SalvarTransacao(transacao,"Completed",bancoParceiro,dataTransacoes.ToString("yyyyMMdd"));
                else
                    SalvarTransacao(transacao,"Failed",bancoParceiro,dataTransacoes.ToString("yyyyMMdd"));
            }
        }
        public static List<Transacao?> LerTodasTransacoesArquivo(string nomeArquivo)
        {
            List <Transacao?> records = new List<Transacao?>{};

            string pathTransacoes = Path.Combine(desktopPath, $"Transactions/Pending/{nomeArquivo}");
            using (var reader = new StreamReader(pathTransacoes))
            using (var csv = new CsvReader(reader, csvConfig))

            while(csv.Read())
                records.Add(csv.GetRecord<Transacao>());
                        
            return records;
        }

        public static void SalvarTransacao(Transacao record,string status, string bancoParceiro, string dataTransacao)
        {
            string pathTransacoes;
            if (status == "Pending")
                pathTransacoes = Path.Combine(desktopPath, $"Transactions/Pending/{bancoParceiro}-{dataTransacao}.csv");
            else
                pathTransacoes = Path.Combine(desktopPath, $"Transactions/{status}/{bancoParceiro}-{dataTransacao}-{status.ToLower()}.csv");

            using (var stream = File.Open(pathTransacoes, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, csvConfig))

            csv.WriteRecords(new List<Transacao>{record});
            
        }
        public static bool AtualizarSaldoConta(int numeroContaAchar, decimal valorAdicionar)
        {   
            bool retorno = false;         
            string[][] todasContas = File.ReadAllLines(filePathUsers).Select(l => l.Split(',')).ToArray();
            List<string> stringFinal = new List<string>{};

            for (int i=0; i<todasContas.Count(); i++)
            {
                if (Int32.Parse(todasContas[i][2]) == numeroContaAchar)
                {
                    decimal saldoAtual;
                    Decimal.TryParse(todasContas[i][3], out saldoAtual);
                    if ((saldoAtual + valorAdicionar)>=0)
                    {
                        todasContas[i][3] = ""+(saldoAtual + valorAdicionar);
                        retorno = true;
                    }
                    else
                        return false;
                    
                stringFinal.Add(string.Join(',',todasContas[i]));
                }
            }
            File.WriteAllLines(filePathUsers, stringFinal);
            return retorno;
        }
    }
}