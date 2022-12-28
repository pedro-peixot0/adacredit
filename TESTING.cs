using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdaCredit
{
    public class Testes
    {
        static void Main()
        {
            
            var user = new Funcionario (
                cpf: 1106166248,
                nomeCompleto: "Pedro Ferreira",
                senha: "senha"
            );  
            
            Usuario? userloaded = Utils.CarregarUsuarioSalvo("pedro.110","senha");
            Console.WriteLine(userloaded.Cpf);
        }
    }


    public class Foo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
