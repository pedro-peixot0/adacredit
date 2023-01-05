namespace AdaCreditBackend
{
    public class Funcionario:Usuario
    {
        //Contrutor para novos Funcionários
        public Funcionario(long cpf,string nomeCompleto, string login, string senha):base(cpf, nomeCompleto, login, senha)
        {
            SalvarUsuarioEmArquivo(this);
        }

        //Contrutor para o CsvHelper
        public Funcionario(
            long cpf,
            string nomeCompleto,
            bool contaAtiva,
            string login,
            string hashSenha,
            string salt
        ):base(cpf, nomeCompleto, contaAtiva, login, hashSenha, salt)
        {
        }
    }
}
