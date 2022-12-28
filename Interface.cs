namespace AdaCredit
{
    public partial class Interface
    {
        //Usuario usuarioLogado;

        public Interface()
        {
            //this.usuarioLogado = usuarioEntrado;
            ReceberInput(
                displayMessage: "Entre o valor em reais",
                maxLength: 28,
                Validador: validadorCharString,
                FormatadorDeInput: FormatadorInputsSenha
            );
        }
    }
}