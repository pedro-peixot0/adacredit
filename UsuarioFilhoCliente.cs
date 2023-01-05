namespace AdaCreditBackend
{
    public class Cliente:Usuario
    {
        public ContaBancaria Conta {get;set;}
        
        //Contrutor para novos Clientes
        public Cliente(string nomeCompleto, string login,string senha, long cpf):base(cpf, nomeCompleto, login,senha)
        {
            this.Conta = new ContaBancaria();
            SalvarUsuarioEmArquivo(this);
        }

        //Contrutor para o CsvHelper
        public Cliente(
            ushort numeroBanco, 
            ushort numeroAgencia, 
            int numeroConta, 
            decimal valorEmConta,
            long cpf,
            string nomeCompleto, 
            bool contaAtiva,
            string login, 
            string hashSenha, 
            string salt
        ):base(cpf, nomeCompleto, contaAtiva, login, hashSenha, salt)
        {
            this.Conta = new ContaBancaria(
                numeroBanco, numeroAgencia, numeroConta, valorEmConta
            );
        }

    }
}
