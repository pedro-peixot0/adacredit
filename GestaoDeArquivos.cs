using static BCrypt.Net.BCrypt;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdaCreditBackend
{
    public partial class Utils
    { 
        public static string  desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static CsvHelper.Configuration.CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture){HasHeaderRecord = false};
        public static string currrentDir = Directory.GetCurrentDirectory();
        private static string filePathUsers = Path.Combine(currrentDir, "data/users.csv");

        public static List <string[]> ListaTransacoesFalhas()
        {
            List <string[]> todasTransacoesFalhas = new List<string[]>{};

            string dir = Path.Combine(desktopPath, "Transactions/Failed");
            string[] allFiles = Directory.GetFiles(dir);
            foreach (string file in allFiles)
            {
                using (var reader = new StreamReader(file))
                using (var csv = new CsvParser(reader, csvConfig))
                while(csv.Read())
                {
                    todasTransacoesFalhas.Add(csv.Record);
                }
            }
                       
            return todasTransacoesFalhas;
        }
        public static void ProcessarTodas()
        {
            string dir = Path.Combine(desktopPath, "Transactions/Pending");
            string[] allFiles = Directory.GetFiles(dir);

            foreach (string file in allFiles)
            {
                if (file.Substring(file.Length-3,3)=="csv")
                {
                    string fileName = Path.GetFileName(file);
                    var allTransacoes = LerTodasTransacoesArquivo(fileName);
                    string nomeBancoLido = fileName.Substring(0,fileName.Length-13);
                    string stringData = fileName.Substring(fileName.Length-12,8);     
                    DateTime data = DateTime.ParseExact(stringData,"yyyyMMdd",CultureInfo.InvariantCulture);
                    ProcessarTransacoes(allTransacoes,(DateTime)data,nomeBancoLido);
                    File.Delete(Path.Combine(dir,fileName));
                }
            }
        }
        public static void ProcessarTransacoes(List<Transacao?> listaTransacoes, DateTime dataTransacoes, string bancoParceiro)
        {
            static bool ChecarSeClienteExiste(int numeroConta)
            {
                var listaUsuarios = Usuario.LerTodosUsuários();
                foreach (Usuario usuario in listaUsuarios)
                {
                    if (usuario is Cliente cliente)
                    {
                        if (cliente.Conta.NumeroConta == numeroConta)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            
            DateTime dataLimiteGratuidade = new DateTime(2022,11,30);

            foreach (Transacao? transacao in listaTransacoes)
            {
                decimal taxa = 0M;
                //pulando transaçoes que não envolvem o banco
                if((transacao.NumeroBancoOrigem != 777 && transacao.NumeroBancoDestino != 777))
                {
                    SalvarTransacao(transacao,"Completed",bancoParceiro,dataTransacoes);
                    continue;
                }
                if(
                    transacao.NumeroBancoOrigem == transacao.NumeroBancoDestino && 
                    transacao.NumeroAgenciaOrigem == transacao.NumeroAgenciaDestino &&
                    transacao.NumeroContaOrigem == transacao.NumeroContaDestino
                )
                {
                    SalvarTransacao(transacao,"Failed",bancoParceiro,dataTransacoes,"Mesma Origem e Destino");
                    continue;
                }
                //remover transações inválidas
                if(transacao.TipoTransacao == "TEF" && transacao.NumeroBancoOrigem != transacao.NumeroBancoDestino)
                {
                    SalvarTransacao(transacao,"Failed",bancoParceiro,dataTransacoes,"TEF entre bancos diferentes");
                    continue;
                }

                //adicionar tarifas às transações de crédito do banco e realizar transação
                if  ((transacao.NumeroBancoOrigem == 777) && (transacao.SentidoTransacao == 1))
                {
                    if (dataTransacoes > dataLimiteGratuidade)
                    {
                        switch(transacao.TipoTransacao)
                        {
                            case "TED":
                                taxa = 5M; break;
                            case "DOC":
                                taxa = transacao.ValorTransacao * 0.01M >= 5 ? transacao.ValorTransacao * 0.01M : 5; break;
                            case "TEF":
                                taxa = 0; break;
                            default:
                                SalvarTransacao(transacao,"Failed",bancoParceiro,dataTransacoes,"Tipo de Transação inválido"); continue;
                        }
                    }
                }
                //a ideia é tentar tirar o dinheiro primeiro
                if (transacao.NumeroBancoOrigem == 777 && transacao.NumeroBancoDestino == 777) 
                {  
                    if (transacao.SentidoTransacao == 1)
                    {
                        if (ChecarSeClienteExiste(transacao.NumeroContaOrigem))
                        {
                            if(ChecarSeClienteExiste(transacao.NumeroContaDestino))
                            {
                                if (AdicionarSaldoConta(transacao.NumeroContaDestino,-(transacao.ValorTransacao+taxa)))
                                {
                                    AdicionarSaldoConta(transacao.NumeroContaOrigem, transacao.ValorTransacao);
                                    SalvarTransacao(transacao,"Completed",bancoParceiro,dataTransacoes);
                                }
                                else
                                    SalvarTransacao(transacao,"Failed",bancoParceiro,dataTransacoes,"Saldo insuficiente Destino");
                            }
                            else
                                SalvarTransacao(transacao,"Failed",bancoParceiro,dataTransacoes,"Conta Destino Inexistente");
                        }
                        else
                            SalvarTransacao(transacao,"Failed",bancoParceiro,dataTransacoes,"Conta Origem Inexistente");
                    }
                    else if (transacao.SentidoTransacao == 0)
                    {
                        if (ChecarSeClienteExiste(transacao.NumeroContaOrigem))
                        {
                            if(ChecarSeClienteExiste(transacao.NumeroContaDestino))
                            {
                                if (AdicionarSaldoConta(transacao.NumeroContaOrigem,-(transacao.ValorTransacao+taxa)))
                                {
                                    AdicionarSaldoConta(transacao.NumeroContaDestino, transacao.ValorTransacao);
                                    SalvarTransacao(transacao,"Completed",bancoParceiro,dataTransacoes);
                                }
                                else
                                    SalvarTransacao(transacao,"Failed",bancoParceiro,dataTransacoes,"Saldo insuficiente Origem");
                            }
                            else
                                SalvarTransacao(transacao,"Failed",bancoParceiro,dataTransacoes,"Conta Destino Inexistente");
                        }
                        else
                            SalvarTransacao(transacao,"Failed",bancoParceiro,dataTransacoes,"Conta Origem Inexistente");
                    }
                }
                else if (transacao.NumeroBancoOrigem == 777)
                {
                    if (transacao.SentidoTransacao == 1)
                    {
                        if (AdicionarSaldoConta(transacao.NumeroContaOrigem, transacao.ValorTransacao))
                            SalvarTransacao(transacao,"Completed",bancoParceiro,dataTransacoes);
                        else    
                            SalvarTransacao(transacao,"Failed",bancoParceiro,dataTransacoes,"Conta Origem Inexistente");
                    }
                    else if (transacao.SentidoTransacao == 0)
                    {
                        if(ChecarSeClienteExiste(transacao.NumeroContaOrigem))
                        {
                            if(AdicionarSaldoConta(transacao.NumeroContaOrigem,-(transacao.ValorTransacao+taxa)))
                                SalvarTransacao(transacao,"Completed",bancoParceiro,dataTransacoes);
                            else
                                SalvarTransacao(transacao,"Failed",bancoParceiro,dataTransacoes,"Saldo insuficiente Origem");
                        }
                        else
                            SalvarTransacao(transacao,"Failed",bancoParceiro,dataTransacoes,"Conta Origem Inexistente");
                    }                        
                }
                else if(transacao.NumeroBancoDestino == 777)
                {
                    if (transacao.SentidoTransacao == 1)
                    {
                        if(ChecarSeClienteExiste(transacao.NumeroContaDestino))
                        {
                            if(AdicionarSaldoConta(transacao.NumeroContaDestino,-(transacao.ValorTransacao+taxa)))
                                SalvarTransacao(transacao,"Completed",bancoParceiro,dataTransacoes);
                            else
                                SalvarTransacao(transacao,"Failed",bancoParceiro,dataTransacoes,"Saldo insuficiente Destino");
                        }
                        else
                            SalvarTransacao(transacao,"Failed",bancoParceiro,dataTransacoes,"Conta Destino Inexistente");
                    }
                    else if (transacao.SentidoTransacao == 0)
                    {
                        if (AdicionarSaldoConta(transacao.NumeroContaDestino, transacao.ValorTransacao))
                            SalvarTransacao(transacao,"Completed",bancoParceiro,dataTransacoes);
                        else    
                            SalvarTransacao(transacao,"Failed",bancoParceiro,dataTransacoes,"Conta Origem Inexistente");
                    }  
                }
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

        public static void SalvarTransacao(Transacao record,string status, string bancoParceiro, DateTime dataTransacao, string resolucao = "transação aprovada")
        {
            string pathTransacoes;
            if (status == "Pending")
                pathTransacoes = Path.Combine(desktopPath, $"Transactions/Pending/{bancoParceiro.ToLower().Replace(' ','-')}-{dataTransacao.ToString("yyyyMMdd")}.csv");
            else
                pathTransacoes = Path.Combine(desktopPath, $"Transactions/{status}/{bancoParceiro.ToLower().Replace(' ','-')}-{dataTransacao.ToString("yyyyMMdd")}-{status.ToLower()}.csv");

            record.Resolucao = resolucao;

            using (var stream = File.Open(pathTransacoes, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, csvConfig))

            csv.WriteRecords(new List<Transacao>{record});
        }
        public static bool AdicionarSaldoConta(int numeroContaAchar, decimal valorAdicionar)
        {   
            bool retorno = false;

            var listaUsuarios = Usuario.LerTodosUsuários();
            var listaClientes = new List<Cliente>{};
            var listaFuncionarios = new List<Funcionario>{};
            
            foreach (Usuario? usuario in listaUsuarios)
            {
                if (usuario is Cliente cliente)
                {
                    if (cliente.Conta.NumeroConta == numeroContaAchar)
                    {
                        cliente.Conta.ValorEmConta += valorAdicionar;
                        
                        if (cliente.Conta.ValorEmConta < 0)
                            return false;
                        else
                            retorno = true;
                    }
                    listaClientes.Add(cliente);
                }
                else if (usuario is Funcionario funcionario)
                    listaFuncionarios.Add(funcionario);
            }

            if (retorno == true)
            {
                using (var writer = new StreamWriter("data/users.csv"))
                using (var csv = new CsvWriter(writer, csvConfig))
                {
                    csv.WriteRecords(listaClientes);
                    csv.WriteRecords(listaFuncionarios);
                }
            }

            return retorno;
        }
        public static List<string[]> UltimoAcessoFuncionarios()
        {
            var todosUsuarios = AdaCreditBackend.Usuario.LerTodosUsuários();
            var listaFuncionarios = new List<AdaCreditBackend.Funcionario>{};
            foreach(var user in todosUsuarios)
            {
                if (user is Funcionario funcionario)
                {
                    if (funcionario.ContaAtiva==true)
                        listaFuncionarios.Add(funcionario);
                }
            }

            var filePath = Path.Combine(currrentDir, "data/logs.csv");
            var matrix = File.ReadAllLines(filePath).Select(linha => linha.Split(',').ToArray()).ToArray();
            List<string[]> finalMatrix = new List<string[]>{};
            
            
            for(int i = matrix.Count()-1;i>=0;i--)
            {
                foreach (var func in listaFuncionarios)
                {
                    if (matrix[i][0] == func.Login && func.ContaAtiva==true)
                    {
                        bool alredyAdded = false;
                        foreach(var linha in finalMatrix)
                        {
                            if (matrix[i][0]==linha[0])
                                alredyAdded = true;
                        }
                        if (!alredyAdded)
                            finalMatrix.Add(matrix[i]);
                    }
                }
            }
            return finalMatrix;
        }
    }
}