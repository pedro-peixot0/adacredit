using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdaCredit
{
    public class Testes
    {
        static void Main()
        {
            
            // var user = new Cliente (
            //     cpf: 1106166248,
            //     nomeCompleto: "Pedro Ferreira",
            //     senha: "senha"
            // );  
            
            // Usuario? userloaded = Usuario.CarregarUsuarioSalvo("pedro.110","senha");
            // Console.WriteLine(userloaded.Cpf);
            var tran = new Transacao(
                numeroBancoOrigem: 777,
                numeroAgenciaOrigem: 1,
                numeroContaOrigem: 780480,
                numeroBancoDestino: 666,
                numeroAgenciaDestino: 1,
                numeroContaDestino: 435345,
                tipoTransacao: "TED",
                sentidoTransacao: 0,
                valorTransacao:30.52M
            );
            var prop = typeof(Transacao).GetProperty("NumeroBancoOrigem");
            prop.SetValue(tran,(ushort)0);
            Console.WriteLine(tran.NumeroBancoOrigem);
            string nomeBanco = "Banco PAN";
            nomeBanco = nomeBanco.ToLower().Replace(' ','-');
            Utils.SalvarTransacao(tran, "Pending", nomeBanco, DateTime.Now.ToString("yyyyMMdd"));
        }
    }
}