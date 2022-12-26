namespace AdaCredit
{
    public class Cliente:Usuario
    {
        ContaBancaria conta;
        public Cliente(string login, string senha)
        {
            this.login = login;
            this.senha = senha;
        }
    }
}
