using static BCrypt.Net.BCrypt;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdaCreditBackend
{
    public partial class Usuario
    {
        public long Cpf {get;set;}
        public string NomeCompleto{get;set;}
        public bool ContaAtiva {get;set;} = true;
        public string Login{get;set;}
        public string HashSenha{get;set;}
        public string Salt{get;set;}
        public Usuario(long cpf,string nomeCompleto, bool contaAtiva, string login, string hashSenha, string salt)
        {
            this.Cpf = cpf;
            this.NomeCompleto = nomeCompleto;
            this.ContaAtiva = contaAtiva;
            this.Login = login;
            this.Salt = salt;
            this.HashSenha = hashSenha;
        }
        public Usuario(long cpf,string nomeCompleto, string login, string senha)
        {
            this.Cpf = cpf;
            this.NomeCompleto = nomeCompleto;
            this.Login = login;
            this.Salt = GerarSalt();
            this.HashSenha = GerarHashSenha(senha,this.Salt);
        }
        public string GerarStringLogin ()
        {
            //AINDA PRECISA VERIFICAR SE O LOGIN EXISTE
            return $"{this.NomeCompleto.Split(' ')[0].ToLower()}.{(int) (this.Cpf/10_000_000)}";
        }
        public bool TrocarSenha(string senhaAntiga, string novaSenha, string confirmacaoSenha)
        {
            if ((ChecarSenhaContraHash(senhaAntiga,HashSenha,Salt)) || (novaSenha != confirmacaoSenha))
                return false;

            this.Salt = GerarSalt();
            this.HashSenha = GerarHashSenha(novaSenha,this.Salt);
            return true;
        }
    }
}
