using static BCrypt.Net.BCrypt;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdaCreditBackend
{
    public partial class Usuario
    {
        public static string currrentDir = Directory.GetCurrentDirectory();
        public static CsvHelper.Configuration.CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture){HasHeaderRecord = false};
        public static string filePathUsers = Path.Combine(currrentDir, "data/users.csv");
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

        public static void AlterarDadosUsuario (Usuario usuarioAlterar, string nomeCompleto, long cpf)
        {
            List <Usuario?> listaUsuarios = LerTodosUsuários();
            var listaClientes = new List<Cliente>{};
            var listaFuncionarios = new List<Funcionario>{};

            foreach (Usuario? usuario in listaUsuarios)
            {
                if (usuario is Cliente cliente)
                {
                    if (cliente.Login == usuarioAlterar.Login)
                    {
                        cliente.Cpf = cpf;
                        cliente.NomeCompleto = nomeCompleto;
                    }
                    
                    listaClientes.Add(cliente);
                }
                    
                else if (usuario is Funcionario funcionario)
                {
                    if (funcionario.Login == usuarioAlterar.Login)
                    {
                        funcionario.Cpf = cpf;
                        funcionario.NomeCompleto = nomeCompleto;
                    }
                    
                    listaFuncionarios.Add(funcionario);
                }
            }   

            using (var writer = new StreamWriter("data/users.csv"))
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                csv.WriteRecords(listaClientes);
                csv.WriteRecords(listaFuncionarios);
            }
        }
        public static void AlterarSenhaUsuario (Usuario usuarioAlterar, string senhaAntiga, string novaSenha)
        {
            List <Usuario?> listaUsuarios = LerTodosUsuários();
            var listaClientes = new List<Cliente>{};
            var listaFuncionarios = new List<Funcionario>{};


            foreach (Usuario? usuario in listaUsuarios)
            {
                if (usuario is Cliente cliente)
                {
                    if (
                        cliente.Login == usuarioAlterar.Login && 
                        ChecarSenhaContraHash(senhaAntiga, cliente.Salt, cliente.HashSenha)
                    )
                        cliente.HashSenha = GerarHashSenha(novaSenha, usuario.Salt);                      
                    
                    listaClientes.Add(cliente);
                }
                    
                else if (usuario is Funcionario funcionario)
                {
                    if (
                        funcionario.Login == usuarioAlterar.Login && 
                        ChecarSenhaContraHash(senhaAntiga, funcionario.Salt, funcionario.HashSenha)
                    )
                        funcionario.HashSenha = GerarHashSenha(novaSenha, funcionario.Salt);
                    
                    listaFuncionarios.Add(funcionario);
                }
            }   

            using (var writer = new StreamWriter("data/users.csv"))
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                csv.WriteRecords(listaClientes);
                csv.WriteRecords(listaFuncionarios);
            }

        }
        public static Usuario? CarregarUsuarioSalvo(string login, string senha)
        {
            var listaUsuarios = LerTodosUsuários();
            
            foreach(Usuario? usuario in listaUsuarios)
            {
                if (login == usuario.Login && ChecarSenhaContraHash(senha, usuario.Salt,usuario.HashSenha))
                {
                    if (usuario is Cliente cliente)
                        return cliente;
                    else if (usuario is Funcionario funcionario)
                        return funcionario;
                }
            }

            return null;
        }
        public static List<Usuario?> LerTodosUsuários()
        {
            List <Usuario?> records = new List<Usuario?>{};

            using (var reader = new StreamReader(filePathUsers))
            using (var csv = new CsvReader(reader, csvConfig))

            while(csv.Read())
            {
                try
                {
                    records.Add(csv.GetRecord<Cliente?>());
                }
                catch
                {
                    records.Add(csv.GetRecord<Funcionario?>());
                }
            }
                       
            return records;
        }
        
        public static List<string[]> LerTodosUsuáriosAtivos(string ativo)
        {
            List <string[]> todosUsuatiosAtivos = new List<string[]>{};

            using (var reader = new StreamReader(filePathUsers))
            using (var csv = new CsvParser(reader, csvConfig))
            while(csv.Read())
            {
                try
                {
                    var cli = csv.Record;
                    if (cli[6] == ativo)
                        todosUsuatiosAtivos.Add(cli);
                }
                catch
                {}
            }
                       
            return todosUsuatiosAtivos;
        }  

        static public bool ChecarSeloginExiste(string login)
            {
                var listaUsuarios = Usuario.LerTodosUsuários();
                foreach (Usuario usuario in listaUsuarios)
                {
                    if (usuario.Login == login)
                    {
                        return true;
                    }
                }
                return false;
            }
        static public void DesativarConta(Usuario usuarioDesativar)
        {
            List <Usuario?> listaUsuarios = LerTodosUsuários();
            var listaClientes = new List<Cliente>{};
            var listaFuncionarios = new List<Funcionario>{};

            foreach (Usuario? usuario in listaUsuarios)
            {
                if (usuario is Cliente cliente)
                {
                    if (cliente.Login == usuarioDesativar.Login)
                        cliente.ContaAtiva = false;                   
                    listaClientes.Add(cliente);
                }
                else if (usuario is Funcionario funcionario)
                {
                    if (funcionario.Login == usuarioDesativar.Login)
                        funcionario.ContaAtiva = false;                     
                    listaFuncionarios.Add(funcionario);
                }
            }   

            using (var writer = new StreamWriter("data/users.csv"))
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                csv.WriteRecords(listaClientes);
                csv.WriteRecords(listaFuncionarios);
            }
        }
    }
}
