using Terminal.Gui;

namespace AdaCreditGUI
{
	public partial class ExampleWindow : Window {

		public ExampleWindow ()
		{	
			Label adaLogo;
			TextField usernameText;
			Terminal.Gui.ColorScheme greenOnBlack = new Terminal.Gui.ColorScheme()
			{
				Normal = new Terminal.Gui.Attribute(Terminal.Gui.Color.Green, Terminal.Gui.Color.Black),
				HotNormal = new Terminal.Gui.Attribute(Terminal.Gui.Color.BrightGreen, Terminal.Gui.Color.Black),
				Focus = new Terminal.Gui.Attribute(Terminal.Gui.Color.Black, Terminal.Gui.Color.Gray),
				HotFocus = new Terminal.Gui.Attribute(Terminal.Gui.Color.BrightGreen, Terminal.Gui.Color.Magenta),
				Disabled = new Terminal.Gui.Attribute(Terminal.Gui.Color.Gray, Terminal.Gui.Color.Black),
			};
			this.ColorScheme = greenOnBlack;
			this.Title = "AdaCredit (Ctrl+Q para sair)";
			adaLogo = new Label(){
				Text =@"
             ..       ..             
       .~JYYJ?7?JJ!?JJ?7?JYY7:.      
    ^YYJ@P.      G#&^      !@B?57    
  .GY. GY       G5 .&:      .&. ~B?  
 .&^  .@.      !&   !&       G5   G5 
 #?   .@       &~    &~      5P    &^
.@.    &~     .&     YP      &!    G5
 @:    ~&     ~#     ~&     ?#     BJ
 PB     7#.   5G     .@    ?B     ^@.
  GG.    :G?. &!      &~ ^GJ     !&^ 
   ~GJ^    ^YG@:      G&57.   .!PY.  
     .!?J??!?5^!777777^?Y!7?JJ?^


              Ada Credit",
			  X = Pos.Center(),
			  Y=0,
			};
			// Create input components and labels
			var usernameLabel = new Label () { 
				Text = "Usuário:",
				X = 5,
				Y = Pos.Bottom(adaLogo)+2, 
			};

			usernameText = new TextField ("") {
				// Position text field adjacent to the label
				X = Pos.Right (usernameLabel) + 1,
				Y = Pos.Top (usernameLabel),
				// Fill remaining horizontal space
				Width = Dim.Fill ()-5,
			};

			var passwordLabel = new Label () {
				Text = "Senha:",
				X = Pos.Left (usernameLabel),
				Y = Pos.Bottom (usernameLabel) + 1
			};

			var passwordText = new TextField ("") {
				Secret = true,
				// align with the text box above
				X = Pos.Left (usernameText),
				Y = Pos.Top (passwordLabel),
				Width = Dim.Fill ()-5,
			};

			// Create login button
			var btnLogin = new Button () {
				Text = "  Entrar  ",
				Y = Pos.Bottom(passwordLabel) + 1,
				// center the login button horizontally
				X = Pos.Center (),
				IsDefault = true,
			};
			var newUser = new Button () {
				Text = " Criar Conta ",
				Y = Pos.Bottom(btnLogin) + 1,
				// center the login button horizontally
				X = Pos.Center (),
				IsDefault = true,
			};

			var primeiroLogin =new JanelaDeNovoUsuario().JanelaDeFuncionario();
			var novaContaCliente = new JanelaDeNovoUsuario().JanelaDeCliente();
			// When login button is clicked display a message popup
			btnLogin.Clicked += () => {
				AdaCreditBackend.Usuario? usuario = AdaCreditBackend.Usuario.CarregarUsuarioSalvo((string) usernameText.Text, (string) passwordText.Text);
				if(usuario != null)
				{
					if (usuario.ContaAtiva == true)
					{
						MessageBox.Query ("Logging In", "Login Successful", "Ok");
						Application.Top.RemoveAll();
						Application.RequestStop();
						
						if (usuario is AdaCreditBackend.Cliente cliente)
							Application.Run(new ClienteLogado(cliente));						
						else if(usuario is AdaCreditBackend.Funcionario funcionario)
						{
							using (var writer = new StringWriter())
							{
								string path = Path.Combine(AdaCreditBackend.Utils.currrentDir,"data/logs.csv");
								writer.Write($"\n{funcionario.Login},{funcionario.Cpf},{DateTime.Now}");
								System.IO.File.AppendAllText(path, writer.ToString());
							} 
							Application.Run(new FuncionarioLogado(funcionario));	
						}
					}
					else
						MessageBox.ErrorQuery ("Logging In", "Usuário desativado", "Ok");
				}
				else if(usernameText.Text =="user" && passwordText.Text == "pass")
				{
					primeiroLogin.Visible = true;
				}
				else {
					MessageBox.ErrorQuery ("Logging In", "Usuário ou Senha incorretos", "Ok");
				}
			};
			newUser.Clicked += () => {
				novaContaCliente.Visible=true;
			};
			// Add the views to the Window
			this.Add (adaLogo,usernameLabel, usernameText, passwordLabel, passwordText, btnLogin, newUser,primeiroLogin,novaContaCliente);
		}
	}
}
