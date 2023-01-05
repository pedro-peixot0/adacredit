namespace AdaCreditBackend
{
    public class ContaBancaria
    {
        public ushort NumeroBanco {get;set;} = 777;
        public ushort NumeroAgencia {get;set;} = 1;
        public int NumeroConta {get;set;}
        public decimal ValorEmConta {get;set;}

        public ContaBancaria(ushort numeroBanco, ushort numeroAgencia, int numeroConta, decimal valorEmConta)
        {
            this.NumeroBanco = numeroBanco;
            this.NumeroAgencia = numeroAgencia;
            this.NumeroConta = numeroConta;
            this.ValorEmConta = valorEmConta;
        }
        public ContaBancaria()
        {
            Random random = new Random();
            this.NumeroConta = random.Next (0,999999);
        }
    }
}
