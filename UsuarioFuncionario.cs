namespace AdaCredit
{
    public class Funcionario:Usuario
    {
        //Contrutor para novos Funcionários
        public Funcionario(long cpf,string nomeCompleto, string senha):base(cpf, nomeCompleto, senha)
        {
            Utils.SalvarUsuarioEmArquivo(this);
        }

        //Contrutor para o CsvHelper
        public Funcionario(
            long cpf,
            string nomeCompleto,
            string login,
            string hashSenha,
            string salt
        ):base(cpf, nomeCompleto, login, hashSenha, salt)
        {
        }
    }
}
