using Terminal.Gui;

namespace AdaCreditGUI
{
	public partial class ClienteLogado : Terminal.Gui.Window {
		public Terminal.Gui.ColorScheme greenOnBlack = new Terminal.Gui.ColorScheme()
		{
			Normal = new Terminal.Gui.Attribute(Terminal.Gui.Color.Green, Terminal.Gui.Color.Black),
			HotNormal = new Terminal.Gui.Attribute(Terminal.Gui.Color.BrightGreen, Terminal.Gui.Color.Black),
			Focus = new Terminal.Gui.Attribute(Terminal.Gui.Color.Green, Terminal.Gui.Color.DarkGray),
			HotFocus = new Terminal.Gui.Attribute(Terminal.Gui.Color.BrightGreen, Terminal.Gui.Color.Magenta),
			Disabled = new Terminal.Gui.Attribute(Terminal.Gui.Color.Gray, Terminal.Gui.Color.Black),
		};
		public Terminal.Gui.Label labelBoasVindas;
		public Terminal.Gui.FrameView janelaDeSaldo;
		public Terminal.Gui.FrameView janelaDeDados;
		public  ClienteLogado (AdaCreditBackend.Cliente cliente)
		{
			ColorScheme = greenOnBlack;
			Title = "AdaCredit (Ctrl+Q para sair)";
			
			labelBoasVindas = new Label(){
				Text = $"Olá {cliente.NomeCompleto.Split(" ")[0]}, bem vindo de volta ao AdaCredit.",
				X = 2,
				Y = 1,
			};
			
			CriarJanelaDeSaldo(cliente.Conta.ValorEmConta);
			CriarJanelaDeDados(cliente.Conta);
			var janelaAtualizarDados = new JanelaDeNovoUsuario().AtualizarUsuario(cliente);
			var janelaAtualizarSenha = new JanelaDeNovoUsuario().AtualizarSenhaUsuario(cliente);

			var botaoAlterarDados = new Terminal.Gui.Button(){
				Text = "Alterar Dados da Conta",
				X = Pos.Center(),
				Y = Pos.Bottom(janelaDeDados)+1,
				Height = 1,
				ColorScheme = greenOnBlack,
				
			};
			botaoAlterarDados.Clicked += () => {
				janelaAtualizarDados.Visible=true;
			};

			var botaoAlterarSenha = new Terminal.Gui.Button(){
				Text = "Alterar Senha",
				X = Pos.Center(),
				Y = Pos.Bottom(botaoAlterarDados)+1,
				Height = 1,
				ColorScheme = greenOnBlack,
			};
			botaoAlterarSenha.Clicked += () => {
				janelaAtualizarSenha.Visible=true;
			};

			var botaoDesativarConta = new Terminal.Gui.Button(){
				Text = "Desativar Conta",
				X = Pos.Center(),
				Y = Pos.Bottom(botaoAlterarSenha)+1,
				Height = 1,
				ColorScheme = greenOnBlack,
			};
			botaoDesativarConta.Clicked += () => {
				var validacao = MessageBox.Query ("Desativação de Conta", "Você realmente deseja desativar sua Conta?", "Sim", "Não");

				if(validacao == 0)
				{
					AdaCreditBackend.Usuario.DesativarConta(cliente);
					MessageBox.Query ("Desativação de Conta", "conta Desativada com sucesso", "OK");
					Application.RequestStop();
					Application.Top.RemoveAll();
					Application.Run<AdaCreditGUI.ExampleWindow> ();
				}
				else
				{
					Application.RequestStop();
					Application.Top.RemoveAll();
					Application.Run(new ClienteLogado(cliente));
				}
			};
		
			Add(labelBoasVindas,janelaDeSaldo,janelaDeDados,botaoAlterarDados,botaoAlterarSenha,botaoDesativarConta,janelaAtualizarDados,janelaAtualizarSenha);
		}
		public void CriarJanelaDeSaldo(decimal saldo)
		{
			janelaDeSaldo = new FrameView ()
			{
				Title = "Conta",
				ColorScheme = greenOnBlack,
				X = Pos.Center(),
				Y = Pos.Bottom(labelBoasVindas)+1,
				Height = 3,
				Width = Dim.Fill(2),
				Border={
					BorderStyle = Terminal.Gui.BorderStyle.Single,
					BorderBrush = Terminal.Gui.Color.Magenta,					
				},
				ClearOnVisibleFalse = true,
				Visible = true,
			};

			var labelSaldo = new Label () {
				Text = $"Saldo em Conta: R$ {string.Format("{0:N2}",saldo)}",
				X = Pos.Center(),
				Y = Pos.Center()
			};

			janelaDeSaldo.Add(labelSaldo);
		}

		public void CriarJanelaDeDados(AdaCreditBackend.ContaBancaria conta)
		{
			janelaDeDados = new FrameView ()
			{
				Title = "Dados da Conta",
				ColorScheme = greenOnBlack,
				X = Pos.Center(),
				Y =Pos.Bottom(janelaDeSaldo)+1,
				Height = 3,
				Width = Dim.Fill(2),
				Border={
					BorderStyle = Terminal.Gui.BorderStyle.Single,
					BorderBrush = Terminal.Gui.Color.Magenta,					
				},
				ClearOnVisibleFalse = true,
				Visible = true,
			};

			var labelNumeroBanco = new Label(){
				Text = $"Banco: {conta.NumeroBanco}",
				X = 1,
				Y = Pos.Center(),
			};
			var labelNumeroAgencia= new Label(){
				Text = $"Agência: {Convert.ToString(conta.NumeroAgencia).PadLeft(4,'0')}",
				X = Pos.Center(),
				Y = Pos.Center(),
			};
			string stringConta = Convert.ToString(conta.NumeroConta).PadLeft(6,'0');
			string stringContaFormatada = String.Format("{0}-{1}",stringConta.Substring(0,5),stringConta.Substring(5,1));
			var labelNumeroConta = new Label(){
				Text = $"Conta: {stringContaFormatada}",
				X = Pos.Percent(100)-15,
				Y = Pos.Center(),
			};

			janelaDeDados.Add(labelNumeroBanco,labelNumeroAgencia,labelNumeroConta);
		}
	}
}
