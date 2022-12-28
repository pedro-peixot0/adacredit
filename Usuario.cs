using static BCrypt.Net.BCrypt;

namespace AdaCredit
{
    public class Usuario
    {
        private const int WorkFactor = 12;
        public long Cpf {get;set;}
        public string NomeCompleto{get;set;}
        public bool ContaAtiva {get;set;} = true;
        public string Login{get;set;}
        public string HashSenha{get;set;}
        public string Salt{get;set;}
        public Usuario(long cpf,string nomeCompleto, string login, string hashSenha, string salt)
        {
            this.Cpf = cpf;
            this.NomeCompleto = nomeCompleto;
            this.Login = login;
            this.Salt = salt;
            this.HashSenha = hashSenha;
        }
        public Usuario(long cpf,string nomeCompleto, string senha)
        {
            this.Cpf = cpf;
            this.NomeCompleto = nomeCompleto;
            this.Login = GerarStringLogin();
            this.Salt = Utils.GerarSalt();
            this.HashSenha = Utils.GerarHashSenha(senha,this.Salt);
        }
        public string GerarStringLogin ()
        {
            //AINDA PRECISA VERIFICAR SE O LOGIN EXISTE
            return $"{this.NomeCompleto.Split(' ')[0].ToLower()}.{(int) (this.Cpf/10_000_000)}";
        }
        public bool TrocarSenha(string senhaAntiga, string novaSenha, string confirmacaoSenha)
        {
            if ((Utils.ChecarSenhaContraHash(senhaAntiga,HashSenha,Salt)) || (novaSenha != confirmacaoSenha))
                return false;

            this.Salt = Utils.GerarSalt();
            this.HashSenha = Utils.GerarHashSenha(novaSenha,this.Salt);
            return true;
        }
        public void DesativarConta()
        {
            this.ContaAtiva = false;
        }
    }
}
