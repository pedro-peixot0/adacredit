using Terminal.Gui;

namespace AdaCreditGUI
{
	public partial class FuncionarioLogado : Terminal.Gui.Window {
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
		public  FuncionarioLogado (AdaCreditBackend.Funcionario funcionario)
		{
			ColorScheme = greenOnBlack;
			Title = "AdaCredit (Ctrl+Q para sair)";

			labelBoasVindas = new Label(){
				Text = $"Olá {funcionario.NomeCompleto.Split(" ")[0]}, bem vindo de volta ao AdaCredit.",
				X = 2,
				Y = 1,
			};
			//EXECUTAR TRANSAÇÕES
			var botaoTrasacoes = new Terminal.Gui.Button(){
				Text = "Executar Transacoes",
				X = Pos.Center(),
				Y = Pos.Bottom(labelBoasVindas)+2,
				Height = 1,
				ColorScheme = greenOnBlack,				
			};
			botaoTrasacoes.Clicked += () => {
				var validacao = MessageBox.Query ("Realizar transações", "Você realmente deseja computar as transações pendentes?", "Sim", "Não");

				if(validacao == 0)
				{
					AdaCreditBackend.Utils.ProcessarTodas();
					MessageBox.Query ("Realizar transações", "Transações executadas!", "OK");
				}
			};

			// --------- TABELA TRANSAÇÕES FALHAS
			var janelaTabelaTrans = new GerarTabelas().TransacoesFalhas(funcionario);
			var botaoTabelaTrans = new Terminal.Gui.Button(){
				Text = "Lista de Transações Falhas",
				X = Pos.Center(),
				Y = Pos.Bottom(botaoTrasacoes)+1,
				Height = 1,
				ColorScheme = greenOnBlack,
			};
			botaoTabelaTrans.Clicked += () => {
				janelaTabelaTrans.Visible=true;
			};
			// --------- TABELA ULTIMO LOGIN
			var JanelatabelaAcesso= new GerarTabelas().UltimoAcessoFunc(funcionario);
			var botaoTabelaAcesso = new Terminal.Gui.Button(){
				Text = "Lista Último Acesso Funcionários",
				X = Pos.Center(),
				Y = Pos.Bottom(botaoTabelaTrans)+1,
				Height = 1,
				ColorScheme = greenOnBlack,
			};
			botaoTabelaAcesso.Clicked += () => {
				JanelatabelaAcesso.Visible=true;
			};

			// --------- TABELA CLIENTES ATIVOS
			var JanelatabelaAtivo = new GerarTabelas().FuncionariosAtivos(funcionario,"True");
			var botaoTabelaAtivo = new Terminal.Gui.Button(){
				Text = "Lista de Clientes Ativos",
				X = Pos.Center(),
				Y = Pos.Bottom(botaoTabelaAcesso)+1,
				Height = 1,
				ColorScheme = greenOnBlack,
				
			};
			botaoTabelaAtivo.Clicked += () => {
				JanelatabelaAtivo.Visible=true;
			};

			// --------- TABELA CLIENTES INATIVOS
			var JanelatabelaInativo = new GerarTabelas().FuncionariosAtivos(funcionario,"False");
			var botaoTabelaInativo = new Terminal.Gui.Button(){
				Text = "Lista de Clientes Inativos",
				X = Pos.Center(),
				Y = Pos.Bottom(botaoTabelaAtivo)+1,
				Height = 1,
				ColorScheme = greenOnBlack,
				
			};
			botaoTabelaInativo.Clicked += () => {
				JanelatabelaInativo.Visible=true;
			};

			
			var janelaAtualizarDados = new JanelaDeNovoUsuario().AtualizarUsuario(funcionario);
			var janelaAtualizarSenha = new JanelaDeNovoUsuario().AtualizarSenhaUsuario(funcionario);

			var botaoAlterarDados = new Terminal.Gui.Button(){
				Text = "Alterar Dados da Conta",
				X = Pos.Center(),
				Y = Pos.Bottom(botaoTabelaInativo)+1,
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
					AdaCreditBackend.Usuario.DesativarConta(funcionario);
					MessageBox.Query ("Desativação de Conta", "conta Desativada com sucesso", "OK");
					Application.RequestStop();
					Application.Top.RemoveAll();
					Application.Run<AdaCreditGUI.ExampleWindow> ();
				}
				else
				{
					Application.RequestStop();
					Application.Top.RemoveAll();
					Application.Run(new FuncionarioLogado(funcionario));
				}
			};

			Add(labelBoasVindas
				,botaoTrasacoes
				,janelaTabelaTrans
				,botaoTabelaTrans
				,JanelatabelaAcesso
				,botaoTabelaAcesso
				,JanelatabelaAtivo
				,botaoTabelaAtivo
				,JanelatabelaInativo
				,botaoTabelaInativo
				,botaoAlterarDados
				,botaoAlterarSenha
				,botaoDesativarConta
				,janelaAtualizarDados
				,janelaAtualizarSenha
			);
		}
		
	}
}
