namespace AdaCreditBackend
{
    public class Transacao
    {
        public ushort NumeroBancoOrigem {get;set;}
        public ushort NumeroAgenciaOrigem {get;set;}
        public int NumeroContaOrigem {get;set;}
        public ushort NumeroBancoDestino {get;set;}
        public ushort NumeroAgenciaDestino {get;set;}
        public int NumeroContaDestino {get;set;}
        public string TipoTransacao {get;set;}
        public byte SentidoTransacao {get;set;} // 0 - Débito/Saída, 1 - Crédito/Entrada
        public decimal ValorTransacao {get;set;}
        public string Resolucao {get;set;}
        private string[] TiposDeTransacaoDiponiveis = new string[3]{"DOC", "TED", "TEF"}; 

        public Transacao(
            Cliente cliente, 
            ushort numeroBancoDestino,
            ushort numeroAgenciaDestino,
            int numeroContaDestino,
            byte sentidoTransacao,
            string tipoTransacao,
            decimal valorTransacao)
        {
            this.NumeroBancoOrigem = cliente.Conta.NumeroBanco;
            this.NumeroAgenciaOrigem = cliente.Conta.NumeroAgencia;
            this.NumeroContaOrigem = cliente.Conta.NumeroConta;
            this.NumeroBancoDestino = numeroBancoDestino;
            this.NumeroAgenciaDestino = numeroAgenciaDestino;
            this.NumeroContaDestino = numeroContaDestino;
            this.TipoTransacao = tipoTransacao;
            this.SentidoTransacao = sentidoTransacao;
            this.ValorTransacao = valorTransacao;
            this.Resolucao = "Aguardando aprovação";
        }

        //Construtor para CsvHelper
        public Transacao(
            ushort numeroBancoOrigem,
            ushort numeroAgenciaOrigem,
            int numeroContaOrigem,
            ushort numeroBancoDestino,
            ushort numeroAgenciaDestino,
            int numeroContaDestino,
            string tipoTransacao,
            byte sentidoTransacao,
            decimal valorTransacao,
            string resolucao)
        {
            this.NumeroBancoOrigem = numeroBancoOrigem;
            this.NumeroAgenciaOrigem = numeroAgenciaOrigem;
            this.NumeroContaOrigem = numeroContaOrigem;
            this.NumeroBancoDestino = numeroBancoDestino;
            this.NumeroAgenciaDestino = numeroAgenciaDestino;
            this.NumeroContaDestino = numeroContaDestino;
            this.TipoTransacao = tipoTransacao;
            this.SentidoTransacao = sentidoTransacao;
            this.ValorTransacao = valorTransacao;
            this.Resolucao = resolucao;
        }
    }
}
