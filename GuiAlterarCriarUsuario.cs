using Terminal.Gui;

namespace AdaCreditGUI
{
	public class JanelaDeNovoUsuario:Window
	{
		protected Terminal.Gui.ColorScheme greenOnBlack = new Terminal.Gui.ColorScheme()
		{
			Normal = new Terminal.Gui.Attribute(Terminal.Gui.Color.Green, Terminal.Gui.Color.Black),
			HotNormal = new Terminal.Gui.Attribute(Terminal.Gui.Color.BrightGreen, Terminal.Gui.Color.Black),
			Focus = new Terminal.Gui.Attribute(Terminal.Gui.Color.Black, Terminal.Gui.Color.Gray),
			HotFocus = new Terminal.Gui.Attribute(Terminal.Gui.Color.BrightGreen, Terminal.Gui.Color.Magenta),
			Disabled = new Terminal.Gui.Attribute(Terminal.Gui.Color.Gray, Terminal.Gui.Color.Black),
		};
		Terminal.Gui.FrameView JanelaBase;
		Terminal.Gui.Label labelNome; Terminal.Gui.TextField textFieldNome;
		Terminal.Gui.Label labelCpf; Terminal.Gui.TextValidateField textFieldCpf;
		Terminal.Gui.Label labelLogin; Terminal.Gui.TextField textFieldLogin;
		Terminal.Gui.Label labelSenha; Terminal.Gui.TextField textFieldSenha;
		Terminal.Gui.Label labelSenha2; Terminal.Gui.TextField textFieldSenha2;
		Terminal.Gui.Button buttonCriar; Terminal.Gui.Button buttonCancelar;
		public void CriarComponentes()
		{
			JanelaBase = new FrameView ()
			{
				Title = "Nova Conta",
				ColorScheme = greenOnBlack,
				X = Pos.Center(),
				Y = 2,
				Height = 22,
				Width = Dim.Fill(30),
				Border={
					BorderStyle = Terminal.Gui.BorderStyle.Single,
					BorderBrush = Terminal.Gui.Color.Magenta,
					Effect3D = true,
					DrawMarginFrame = true,
					
				},
				ClearOnVisibleFalse = true,
				Visible = false,
			};

			labelNome = new Label () {Text = "Nome Completo:  ",X = 5, Y = 2};
 			textFieldNome = new TextField ("") {X = Pos.Right (labelNome) + 1, Y = 2, Width = Dim.Fill (5),};
			labelCpf = new Label () {Text = "CPF:",X = Pos.Left (labelNome),Y = Pos.Bottom (labelNome) + 1};
 			textFieldCpf = new Terminal.Gui.TextValidateField()
			{
	 			Width = Dim.Fill(5), X = Pos.Left (textFieldNome),Y = Pos.Top (labelCpf),
				Provider = new Terminal.Gui.TextValidateProviders.TextRegexProvider(@"^[0-9_\b]*$"),
				Data = "textValidateField",
				Text = ""
			};
			labelLogin = new Label () {Text = "Login:",X = Pos.Left (labelNome),Y = Pos.Bottom (labelCpf) + 1};
 			textFieldLogin = new TextField ("user") {X = Pos.Left (textFieldNome),Y = Pos.Top (labelLogin),Width = Dim.Fill (5),};
			labelSenha = new Label () {Text = "Senha:",X = Pos.Left (labelNome),Y = Pos.Bottom (labelLogin) + 1};
 			textFieldSenha = new TextField ("") {Secret = true,X = Pos.Left (textFieldNome),Y = Pos.Top (labelSenha),Width = Dim.Fill (5),};
			labelSenha2 = new Label () {Text = "Confirmar Senha:",X = Pos.Left (labelNome),Y = Pos.Bottom (labelSenha) + 1};
 			textFieldSenha2 = new TextField ("") {Secret = true,X = Pos.Left (textFieldNome),Y = Pos.Top (labelSenha2),Width = Dim.Fill (5)};

			buttonCriar = new Button () {
				Text = "  Criar Conta  ",
				Y = Pos.Bottom(labelSenha2)+3,
				X = Pos.Center(),
				IsDefault = true,
			};

			buttonCancelar = new Button () {
				Text = "  Cancelar  ",
				Y = Pos.Bottom(buttonCriar)+1,
				X = Pos.Center(),
				IsDefault = true,
			};
		}

		public Terminal.Gui.FrameView JanelaDeFuncionario()
		{
			CriarComponentes();
			JanelaBase.Title = "Criar nova Conta - Funcionário";

			buttonCriar.Clicked += () => {
				if (textFieldCpf.Text.Length==11)
					{
					if (textFieldLogin.Text.Length != 0 && textFieldSenha.Text.Length != 0)
					{
						if (textFieldNome.Text.Split(" ").Length > 1)
						{
							if(AdaCreditBackend.Usuario.ChecarSeloginExiste((string)textFieldLogin.Text))
								MessageBox.ErrorQuery ("Novo Usuário", "O login escolhido já existe, por favor escolha outro", "Ok");
							else if (textFieldSenha.Text ==  textFieldSenha.Text)
							{
								new AdaCreditBackend.Funcionario(
									Int64.Parse((string) textFieldCpf.Text),
									(string) textFieldNome.Text,
									(string) textFieldLogin.Text,
									(string) textFieldSenha.Text
								);
								JanelaBase.Visible = false;
								Application.RequestStop();
								Application.Run<AdaCreditGUI.ExampleWindow> ();	
							}
							else
								MessageBox.ErrorQuery ("Novo Usuário", "As senhas não batem", "Ok");
						}
						else
							MessageBox.ErrorQuery ("Nova Conta", "É necessário adicionar nome e sobrenome", "Ok");
					}
					else
						MessageBox.ErrorQuery ("Nova Conta", "Login e Senha devem ter ao menos 1 caractere", "Ok");
					}
				else
					MessageBox.ErrorQuery ("Nova Conta", "O CPF deve ter 11 caracteres", "Ok");
			};	
			buttonCancelar.Clicked += () => {
				JanelaBase.Visible = false;
				Application.RequestStop();
				Application.Run<AdaCreditGUI.ExampleWindow> ();	
			};		

			JanelaBase.Add(
				labelNome, textFieldNome,
				labelCpf, textFieldCpf,
				labelLogin, textFieldLogin,
				labelSenha, textFieldSenha,
				labelSenha2, textFieldSenha2,
				buttonCriar,buttonCancelar
			);

			return JanelaBase;
		}
		public Terminal.Gui.FrameView JanelaDeCliente()
		{
			CriarComponentes();
			JanelaBase.Title = "Criar novo Conta - Cliente";
 			textFieldLogin.Text = ""; 

			buttonCriar.Clicked += () => {
				if (textFieldCpf.Text.Length==11)
					{
					if (textFieldLogin.Text.Length != 0 && textFieldSenha.Text.Length != 0)
					{
						if (textFieldNome.Text.Split(" ").Length > 1)
						{
							if(AdaCreditBackend.Usuario.ChecarSeloginExiste((string)textFieldLogin.Text))
							{
								MessageBox.ErrorQuery ("Nova Conta", "O login escolhido já existe, por favor escolha outro", "Ok");
							}
							else if (textFieldSenha.Text ==  textFieldSenha2.Text)
							{
								new AdaCreditBackend.Cliente(
									(string) textFieldNome.Text,
									(string) textFieldLogin.Text,		
									(string) textFieldSenha.Text,
									Int64.Parse((string) textFieldCpf.Text)
								);
								JanelaBase.Visible = false;
								Application.RequestStop();
								Application.Run<AdaCreditGUI.ExampleWindow> ();	
							}
							else
								MessageBox.ErrorQuery ("Nova Conta", "As senhas não batem", "Ok");
						}
						else
							MessageBox.ErrorQuery ("Nova Conta", "É necessário adicionar nome e sobrenome", "Ok");
					}
					else
						MessageBox.ErrorQuery ("Nova Conta", "Login e Senha devem ter ao menos 1 caractere", "Ok");
					}
				else
					MessageBox.ErrorQuery ("Nova Conta", "O CPF deve ter 11 caracteres", "Ok");
			};
			buttonCancelar.Clicked += () => {
				JanelaBase.Visible = false;
				Application.RequestStop();
				Application.Run<AdaCreditGUI.ExampleWindow> ();	
			};

			JanelaBase.Add(
				labelNome, textFieldNome,
				labelCpf, textFieldCpf,
				labelLogin, textFieldLogin,
				labelSenha, textFieldSenha,
				labelSenha2, textFieldSenha2,
				buttonCriar,buttonCancelar	
			);

			return JanelaBase;
		}

		public Terminal.Gui.FrameView AtualizarUsuario(AdaCreditBackend.Usuario usuario)
		{
			CriarComponentes();
			JanelaBase.Title = "Atualizar Dados - Cliente";
			JanelaBase.Height = 15;
			textFieldCpf.Text = ""+usuario.Cpf;
			textFieldNome.Text = usuario.NomeCompleto;
			
			buttonCriar.Text = " Salvar ";
			buttonCriar.Y = Pos.Bottom(labelCpf)+3;

			buttonCriar.Clicked += () => {
				AdaCreditBackend.Usuario.AlterarDadosUsuario(
					usuario,
					(string)textFieldNome.Text,
					Int64.Parse((string) textFieldCpf.Text
					)
				);
				usuario.NomeCompleto = (string)textFieldNome.Text;
				usuario.Cpf = Int64.Parse((string) textFieldCpf.Text);

				MessageBox.Query ("Atualizar Dados", "Dados atualizados com sucesso", "Ok");

				JanelaBase.Visible = false;
				Application.RequestStop();
				if (usuario is AdaCreditBackend.Cliente cliente)
					Application.Run(new ClienteLogado(cliente));
				if (usuario is AdaCreditBackend.Funcionario funcionario)
					Application.Run(new FuncionarioLogado(funcionario));	
			};

			buttonCancelar.Clicked += () => {
				Application.RequestStop();
				if (usuario is AdaCreditBackend.Cliente cliente)
					Application.Run(new ClienteLogado(cliente));
				if (usuario is AdaCreditBackend.Funcionario funcionario)
					Application.Run(new FuncionarioLogado(funcionario));	
			};

			JanelaBase.Add(
				labelNome, textFieldNome,
				labelCpf, textFieldCpf,
				buttonCriar,buttonCancelar	
			);

			return JanelaBase;
		}
		public Terminal.Gui.FrameView AtualizarSenhaUsuario(AdaCreditBackend.Usuario usuario)
		{
			CriarComponentes();
			var labelSenhaAntiga = new Label () {Text = "Senha Antiga:   ",X = 5, Y = 2};
			labelSenha.X = Pos.Left(labelSenhaAntiga);labelSenha.Y = Pos.Bottom(labelSenhaAntiga)+1;
			labelSenha2.X = Pos.Left(labelSenhaAntiga); labelSenha2.Y = Pos.Bottom(labelSenha)+1;

			var textFieldSenhaAntiga = new TextField ("") {Secret = true,X = Pos.Right (labelSenhaAntiga) + 1,Y = Pos.Top(labelSenhaAntiga),Width = Dim.Fill (5)};
			textFieldSenha.X = Pos.Right(labelSenhaAntiga) +1; textFieldSenha.Y = Pos.Bottom(textFieldSenhaAntiga)+1;
			textFieldSenha2.X = Pos.Right(labelSenhaAntiga)+1;textFieldSenha2.Y = Pos.Bottom(textFieldSenha)+1;

			buttonCriar.Text = " Salvar ";
			buttonCriar.Y = Pos.Bottom(labelSenha2)+3;
		
			buttonCriar.Clicked += () => {
				if (textFieldSenha.Text == textFieldSenha2.Text)
				{
					if (AdaCreditBackend.Usuario.ChecarSenhaContraHash((string)textFieldSenhaAntiga.Text,usuario.Salt,usuario.HashSenha))
					{
						AdaCreditBackend.Usuario.AlterarSenhaUsuario(
							usuario,
							(string)textFieldSenhaAntiga.Text,
							(string) textFieldSenha2.Text
						);
						usuario.HashSenha = AdaCreditBackend.Usuario.GerarHashSenha((string)textFieldSenhaAntiga.Text,usuario.Salt);

						MessageBox.Query ("Atualizar Senha", "A senha foi alterada com sucesso", "Ok");

						JanelaBase.Visible = false;
						Application.RequestStop();
						if (usuario is AdaCreditBackend.Cliente cliente)
							Application.Run(new ClienteLogado(cliente));
						if (usuario is AdaCreditBackend.Funcionario funcionario)
							Application.Run(new FuncionarioLogado(funcionario));	
					}
					else
						MessageBox.ErrorQuery ("Alterar Senha", "Senha incorreta", "Ok");
				}
				else
					MessageBox.ErrorQuery ("Alterar Senha", "As novas senhas não batem", "Ok");
			};

			buttonCancelar.Clicked += () => {
				Application.RequestStop();
				if (usuario is AdaCreditBackend.Cliente cliente)
					Application.Run(new ClienteLogado(cliente));	
				if (usuario is AdaCreditBackend.Funcionario funcionario)
					Application.Run(new FuncionarioLogado(funcionario));
			};

			JanelaBase.Add(
				labelSenhaAntiga,textFieldSenhaAntiga,
				labelSenha,textFieldSenha,
				labelSenha2,textFieldSenha2,
				buttonCriar,buttonCancelar	
			);

			return JanelaBase;
		}
	}
}