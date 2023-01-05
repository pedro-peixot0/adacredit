using Terminal.Gui;
using System;

namespace AdaCreditGUI
{
	public class GerarTabelas {
		public Terminal.Gui.ColorScheme greenOnBlack = new Terminal.Gui.ColorScheme()
		{
			Normal = new Terminal.Gui.Attribute(Terminal.Gui.Color.Green, Terminal.Gui.Color.Black),
			HotNormal = new Terminal.Gui.Attribute(Terminal.Gui.Color.BrightGreen, Terminal.Gui.Color.Black),
			Focus = new Terminal.Gui.Attribute(Terminal.Gui.Color.Green, Terminal.Gui.Color.DarkGray),
			HotFocus = new Terminal.Gui.Attribute(Terminal.Gui.Color.BrightGreen, Terminal.Gui.Color.Magenta),
			Disabled = new Terminal.Gui.Attribute(Terminal.Gui.Color.Gray, Terminal.Gui.Color.Black),
		};
		public Terminal.Gui.FrameView GerarJanela()
		{
			var JanelaBase = new FrameView ()
			{
				Title = "Nova Conta",
				ColorScheme = greenOnBlack,
				X = Pos.Center(),
				Y = 2,
				Height = 18,
				Width = 90,
				Border={
					BorderStyle = Terminal.Gui.BorderStyle.Single,
					BorderBrush = Terminal.Gui.Color.Magenta,
					Effect3D = true,
					DrawMarginFrame = true,
					
				},
				ClearOnVisibleFalse = true,
				Visible = false,
			};

			return JanelaBase;
		}
		

		public Terminal.Gui.FrameView  FuncionariosAtivos (AdaCreditBackend.Usuario usuario,string ativo)
		{	
			//PEGANDO DADOS
			var listaColunas = new string[5]{"NumeroConta","CPF","Nome Usuario","Login","Valor em Conta"};
			var listaUsuarios = AdaCreditBackend.Usuario.LerTodosUsuáriosAtivos(ativo);
			System.Data.DataTable dt = new System.Data.DataTable();
			
			foreach(var h in listaColunas){
				dt.Columns.Add(h);
			}
			foreach(var line in listaUsuarios) {
				var row = new string[5]{line[2],line[4],line[5],line[7],line[3]};
				dt.Rows.Add(row);
			}
			//CRIANDO JANELA
			var janela = GerarJanela();
			if(ativo == "True")
				janela.Title = "Usuários Ativos";
			if(ativo == "False")
				janela.Title = "Usuários Inativos";

			//CRINADO COMPONENTES
			var tableView = new TableView () {
				X = Pos.Center(),
				Y = 2,
				Width = 80,
				Height = 10,
				
			};

			tableView.Table = dt;

			var buttonCriar = new Button () {
				Text = " OK ",
				Y = Pos.Bottom(tableView)+1,
				X = Pos.Percent(100)-20,
				IsDefault = true,
			};

			buttonCriar.Clicked += () => {
				Application.RequestStop();
				if (usuario is AdaCreditBackend.Cliente cliente)
					Application.Run(new ClienteLogado(cliente));
				if (usuario is AdaCreditBackend.Funcionario funcionario)
					Application.Run(new FuncionarioLogado(funcionario));	
			};

			//
			janela.Add(tableView,buttonCriar);
			return janela;
		}

		public Terminal.Gui.FrameView  TransacoesFalhas (AdaCreditBackend.Usuario usuario)
		{
			//PEGANDO DADOS
			var listaColunas = new string[6]{"Banco/Ag./Conta Origem","Banco/Ag./Conta Destino","Tipo","Sentido","Valor","Razão Falha"};
			var listaTransacoes = AdaCreditBackend.Utils.ListaTransacoesFalhas();
			System.Data.DataTable dt = new System.Data.DataTable();
			
			foreach(var h in listaColunas){
				dt.Columns.Add(h);
			}
			foreach(var line in listaTransacoes) 
			{
				if (line.Length == 10)
				{
					string stringContaOrigem = Convert.ToString(line[2]).PadLeft(6,'0');
					string stringContaDestino= Convert.ToString(line[4]).PadLeft(6,'0');
					var row = new string[6]{
						line[0]+"/"+Convert.ToString(line[1]).PadLeft(4,'0')+"/"+String.Format("{0}-{1}",stringContaOrigem.Substring(0,5),stringContaOrigem.Substring(5,1)),
						line[3]+"/"+Convert.ToString(line[4]).PadLeft(4,'0')+"/"+String.Format("{0}-{1}",stringContaDestino.Substring(0,5),stringContaDestino.Substring(5,1)),
						line[6],
						line[7],
						line[8],
						line[9]
					};
					dt.Rows.Add(row);
				}
			}

			var janela = GerarJanela();
			janela.Title = "Lista de transações Falhas";
			janela.Width = 135;
			var tableView = new TableView () {
				X = Pos.Center(),
				Y = 2,
				Width = 120,
				Height = 10,
			};
			tableView.Table = dt;

			var buttonCriar = new Button () {
				Text = " OK ",
				Y = Pos.Bottom(tableView)+1,
				X = Pos.Percent(100)-20,
				IsDefault = true,
			};

			buttonCriar.Clicked += () => {
				Application.RequestStop();
				if (usuario is AdaCreditBackend.Cliente cliente)
					Application.Run(new ClienteLogado(cliente));
				if (usuario is AdaCreditBackend.Funcionario funcionario)
					Application.Run(new FuncionarioLogado(funcionario));	
			};
			//
			janela.Add(tableView,buttonCriar);
			return janela;
		}

		public Terminal.Gui.FrameView  UltimoAcessoFunc (AdaCreditBackend.Funcionario funcionario)
		{
			//PEGANDO DADOS
			var listaColunas = new string[3]{"Login","CPF","Ultimo Login"};
			var listaAcessos = AdaCreditBackend.Utils.UltimoAcessoFuncionarios();
			System.Data.DataTable dt = new System.Data.DataTable();
			
			foreach(var h in listaColunas){
				dt.Columns.Add(h);
			}
			foreach(var line in listaAcessos) 
			{
				if (line.Length == 3)
				{
					var row = new string[3]{line[0],line[1],line[2]};
					dt.Rows.Add(row);
				}
			}
			var janela = GerarJanela();
			janela.Title = "Lista de transações Falhas";
			janela.Width = 90;
			var tableView = new TableView () {
				X = Pos.Center(),
				Y = 2,
				Width = 80,
				Height = 10,
			};
			tableView.Table = dt;

			var buttonCriar = new Button () {
				Text = " OK ",
				Y = Pos.Bottom(tableView)+1,
				X = Pos.Percent(100)-20,
				IsDefault = true,
			};

			buttonCriar.Clicked += () => {
				Application.RequestStop();
				Application.Run(new FuncionarioLogado(funcionario));	
			};
			//
			janela.Add(tableView,buttonCriar);
			return janela;
		}
		
	}
}
