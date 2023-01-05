// A simple Terminal.Gui example in C# - using C# 9.0 Top-level statements

using Terminal.Gui;
using CsvHelper;


// Defines a top-level window with border and title

public class testes 
{
    static void Main()
    {  
        // CRIANDO PASTAS NECESSÁRIO DE TRANSACTIONS
        string pathTransactions = Path.Combine(AdaCreditBackend.Utils.desktopPath,"Transactions");
        System.IO.Directory.CreateDirectory(pathTransactions);

        string[] listaPastas = new string[3]{"Completed","Pending","Failed"};
        foreach(var f in listaPastas)
        {
            string pathNewFolder = Path.Combine(pathTransactions,f);
            System.IO.Directory.CreateDirectory(pathNewFolder);
        }

        //GERANDO TRANSAÇÕES MOCK
        var listaTodosUsers = AdaCreditBackend.Usuario.LerTodosUsuários();
        var listaClientesAtivos = new List<AdaCreditBackend.Cliente>{};
        foreach(var usuario in listaTodosUsers)
        {
            if (usuario is AdaCreditBackend.Cliente cliente)
            {
                if (cliente.ContaAtiva==true)
                {
                    listaClientesAtivos.Add(cliente);
                }
            }
        }
        Random rnd = new Random();
        string[] TiposDeTransacaoDiponiveis = new string[3]{"DOC", "TED", "TEF"};
        foreach(var cliente in listaClientesAtivos)
        {   
            int tipoTran = rnd.Next(3);
            int destino = rnd.Next(listaClientesAtivos.Count());
            var novaTranscao = new AdaCreditBackend.Transacao(
                cliente:cliente,
                numeroBancoDestino:777,
                numeroAgenciaDestino:1,
                sentidoTransacao:0,
                numeroContaDestino:listaClientesAtivos[destino].Conta.NumeroConta,
                tipoTransacao:TiposDeTransacaoDiponiveis[tipoTran],
                valorTransacao:rnd.Next((int)cliente.Conta.ValorEmConta*2)
            );
            AdaCreditBackend.Utils.SalvarTransacao(novaTranscao,"Pending","ada-credit",DateTime.Now);

            var novaTranscao2 = new AdaCreditBackend.Transacao(
                cliente:cliente,
                numeroBancoDestino:777,
                numeroAgenciaDestino:1,
                sentidoTransacao:1,
                numeroContaDestino:listaClientesAtivos[destino].Conta.NumeroConta,
                tipoTransacao:TiposDeTransacaoDiponiveis[tipoTran],
                valorTransacao:rnd.Next((int)listaClientesAtivos[destino].Conta.ValorEmConta*2)
            );
            AdaCreditBackend.Utils.SalvarTransacao(novaTranscao2,"Pending","ada-credit",DateTime.Now);
             
            var novaTranscao3 = new AdaCreditBackend.Transacao(
                cliente:cliente,
                numeroBancoDestino:111,
                numeroAgenciaDestino:1,
                sentidoTransacao:(byte)rnd.Next(2),
                numeroContaDestino:123345,
                tipoTransacao:TiposDeTransacaoDiponiveis[tipoTran],
                valorTransacao:rnd.Next((int)cliente.Conta.ValorEmConta*2)
            );
            AdaCreditBackend.Utils.SalvarTransacao(novaTranscao3,"Pending","credit-suisse",DateTime.Now);
        }

        Application.Run<AdaCreditGUI.ExampleWindow> ();
        Application.Shutdown ();

    }
}