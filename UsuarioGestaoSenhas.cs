using static BCrypt.Net.BCrypt;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdaCredit
{
    public partial class Usuario
    {
        private const int WorkFactor = 12;
        public static string GerarSalt()
        {
            return GenerateSalt(WorkFactor);
        }        
        public static string GerarHashSenha(string senha, string salt)
        {
            return HashPassword(senha,salt);
        }
        public static bool ChecarSenhaContraHash(string senhaCheck, string salt, string hash)
        {
            string hashCheck = HashPassword(senhaCheck,salt);
            return (hashCheck == hash ? true : false);
        }
    }
}
