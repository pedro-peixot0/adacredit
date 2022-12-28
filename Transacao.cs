namespace AdaCredit
{
    public class Transacao
    {
        public string nunmeroBancoDestino {get;} =  "";
        public string numeroAgenciaDestino {get;} =  "";
        public string numeroContaDestino {get;} =  "";
        public string tipoTransacao {get;} =  "";
        public byte sentidoTransacao {get;} // 0 - Débito/Saída, 1 - Crédito/Entrada
        public decimal valorTransacao {get;}
    }
}
