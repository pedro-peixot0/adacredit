namespace AdaCredit
{
    public class Cliente:Usuario
    {
        public ContaBancaria Conta {get;set;}
        
        //Contrutor para novos Clientes
        public Cliente(string nomeCompleto, string senha, long cpf):base(cpf, nomeCompleto, senha)
        {
            this.Conta = new ContaBancaria();
            this.Login = "pedro";
            Utils.SalvarUsuarioEmArquivo(this);
        }

        //Contrutor para o CsvHelper
        public Cliente(
            ushort numeroBanco, 
            ushort numeroAgencia, 
            int numeroConta, 
            decimal valorEmConta,
            long cpf,
            string nomeCompleto, 
            string login, 
            string hashSenha, 
            string salt
        ):base(cpf, nomeCompleto, login, hashSenha, salt)
        {
            this.Conta = new ContaBancaria(
                numeroBanco, numeroAgencia, numeroConta, valorEmConta
            );
        }

    }
}
