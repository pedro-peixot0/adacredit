namespace AdaCredit
{
    public class ContaBancaria
    {
        public ushort NumeroBanco {get;}
        public ushort NumeroAgencia {get;}
        public int NumeroConta {get;}
        public decimal ValorEmConta {get;}

        public ContaBancaria(
        //    ushort numeroBanco, ushort NumeroAgencia, int NumeroConta
        )
        {
            Random random = new Random();
            this.NumeroBanco = 1;
            this.NumeroAgencia = (ushort) random.Next(0,99999);
            this.NumeroConta = random.Next (0,999999999);
        }
    }
}
