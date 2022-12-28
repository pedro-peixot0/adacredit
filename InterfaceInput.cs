namespace AdaCredit
{
    public partial class Interface
    {
        public List<char> ReceberInput(
            string displayMessage,
            int maxLength,
            Func<char,bool> Validador,
            Func<List<char>,string> FormatadorDeInput
        )
        {
            List<char> listaInputs = new List<char> {};
            ConsoleKeyInfo key;

            do
            {
                Console.Write($"{displayMessage} {FormatadorDeInput(listaInputs)}");
                key = Console.ReadKey();

                if (Validador(key.KeyChar) && listaInputs.Count<=maxLength)      
                    listaInputs.Insert(listaInputs.Count, key.KeyChar);
                else if ((key.Key == ConsoleKey.Backspace) && (listaInputs.Any()))
                    listaInputs.RemoveAt(listaInputs.Count -1);                

                Console.Clear();
            }while(key.Key != ConsoleKey.Enter);

            return listaInputs;
        }
        public bool validadorCharNumero(char caractere)
        {
            if (!Char.IsDigit(caractere))
                return false;

            return true;
        }
        private string FormatadorInputsFloat(List<char> input)
        {
            string inputString = new string(input.ToArray());
            inputString = inputString == "" ? "0" : inputString;
            //tryParse para dizer que o númro é muito grande
            decimal outputDecimal = decimal.Parse(inputString)/100;
            
            return "R$ " + String.Format("{0:N2}", outputDecimal);
        }
        public bool validadorCharString(char caractere)
        {
            if (Char.IsControl(caractere))
                return false;

            return true;
        }
        private string FormatadorInputsSenha(List<char> input)
        {
            return new string(input.Select(c => c = '*').ToArray());
        }
               
    }
}