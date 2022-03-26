using Godot;
using System;
using System.Threading.Tasks;
using BibliotecaViva.DTO.Dominio;

using BibliotecaViva.DTO;
using BibliotecaViva.BLL;
using Onlife.CTRL.Interface;
using BibliotecaViva.BLL.Interface;

namespace Onlife.CTRL
{
	public class DadosCTRL : Control, IDisposableCTRL
	{
		private AcceptDialog PopErro { get; set; }
		private HBoxContainer Coluna { get; set; }

		private JanelaBase Pessoa { get; set; }
		private JanelaBase Registro { get; set; }
		private Node Sobre { get ;set; }
		private Node GPedU { get; set; }

		private IConsultarRegistroBLL RegistroBLL { get; set; }
		private IConsultarPessoaBLL PessoaBLL { get; set; }
		private IBuscarBLL BuscarBLL { get; set; }
		public override void _Ready()
		{
			PopularNodes();
			RealizarInjecaoDeDependencias();
			DesativarFuncoesDeAltoProcessamento();
		}
		public void FecharCTRL()
		{
			PopErro.QueueFree();
			Coluna.QueueFree();
			RegistroBLL.Dispose();
			PessoaBLL.Dispose();
			BuscarBLL.Dispose();
			Sobre.QueueFree();
			GPedU.QueueFree();
			QueueFree();
		}
		private void PopularNodes()
		{
			PopErro = GetNode<AcceptDialog>("./Dados/PopErro");
			Coluna = GetNode<HBoxContainer>("./ScrollContainer/Colunas");
			
			Pessoa = GetNode<JanelaBase>("./JanelaPessoa");
			Registro = GetNode<JanelaBase>("./JanelaRegistro");
			
			Sobre = GetNode<Node>("./JanelaSobre");
			GPedU = GetNode<Node>("./JanelaGPedU");
		}
		private void RealizarInjecaoDeDependencias()
		{
			RegistroBLL = new ConsultarRegistroBLL();
			PessoaBLL = new ConsultarPessoaBLL();
			BuscarBLL = new BuscarBLL(Coluna);
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetProcess(false);
			SetPhysicsProcess(false);
		}
		private void ExibirErro(string mensagem)
		{
			PopErro.DialogText = mensagem;
			PopErro.Popup_();
		}
		private void _on_NovaPistaViva_button_up()
		{
			InstanciarPessoaBox(null, ObterColuna(0), 0);
		}
		private void _on_NovoRegistro_button_up()
		{
			InstanciarPessoaBox(null, ObterColuna(0), 0);
		}
		private void _on_Buscar_button_up()
		{
			// Replace with function body.
		}
		private void _on_Explorar_button_up()
		{
			// Replace with function body.
		}
		private void _on_Sobre_button_up()
		{
			InstanciarOutraJanela(Sobre, ObterColuna(0), 0);
		}
		private void _on_Equipes_button_up()
		{
			//TODO CLAUÊ PRECISAMOS DE UMA JANELA PARA A EQUIPE, NÃO VAI DA PRA FAZER COMO QUERIÍAMOS ANTES
			//InstanciarOutraJanela(Equipe, ObterColuna(0), 0);
		}
		private void _on_GPedU_button_up()
		{
			InstanciarOutraJanela(GPedU, ObterColuna(0), 0);
		}
		private void _on_Contato_button_up()
		{
			//TODO CLAUÊ PRECISAMOS DE UMA JANELA PARA O CONTATO
			//InstanciarOutraJanela(Contato, ObterColuna(0), 0);
		}
		private void InstanciarOutraJanela(Node janelaInstanciada, VBoxContainer container, int coluna)
		{
			var janela = janelaInstanciada.Duplicate();
			container.AddChild(janela);
			janela._Ready();
		}
		private void InstanciarPessoaBox(PessoaObject pessoaObjct, VBoxContainer container, int coluna)
		{
			if (pessoaObjct != null && ValidarPessoaJaInstanciadaNaColuna(pessoaObjct.Pessoa, coluna))
				return;
			var pessoaBox = Pessoa.Duplicate();
			container.AddChild(pessoaBox);
			pessoaBox._Ready();
			var pessoa = pessoaBox.GetNode<JanelaPessoa>("./JanelaPessoa");
			if(pessoaObjct != null)
				pessoa.PopularDados(pessoaObjct.Pessoa);
			pessoa.Dados = this;
			pessoa.Coluna = coluna;
		}
		private void InstanciarRegistroBox(RegistroObject registroObjct, VBoxContainer container, int coluna)
		{
			if (registroObjct != null && ValidarRegistroJaInstanciadoNaColuna(registroObjct.Registro, coluna))
				return;
			var registroBox = Registro.Duplicate();
			container.AddChild(registroBox);
			registroBox._Ready();
			var registro = registroBox.GetNode<JanelaRegistro>("./JanelaRegistro");
			if (registroBox != null)
				registro.PopularDados(registroObjct.Registro);
			registro.Dados = this;
			registro.Coluna = coluna;
		}
		private void InstanciarColuna()
		{
			BuscarBLL.InstanciarColuna();
		}
		private bool ValidarPessoaJaInstanciadaNaColuna(PessoaDTO pessoa, int coluna)
		{
			var pessoas = ObterColuna(coluna).GetChildren();
			foreach (var pessoaBox in pessoas)
			{
				if ((pessoaBox as JanelaPessoa)?.CodPessoa == pessoa.Codigo)
					return true;
			}
			return false;
		}
		private bool ValidarRegistroJaInstanciadoNaColuna(RegistroDTO registro, int coluna)
		{
			var registros = ObterColuna(coluna).GetChildren();
			foreach (var registroBox in registros)
			{
				if ((registroBox as JanelaRegistro)?.CodigoRegistro == registro.Codigo)
					return true;
			}
			return false;
		}
		private VBoxContainer ObterColuna(int coluna)
		{
			if (BuscarBLL.ValidarColuna(coluna))
			{
				BuscarBLL.InstanciarColuna();
				System.Threading.Thread.Sleep(100);
			}
			return Coluna.GetChild(coluna).GetChild<VBoxContainer>(0);
		}
		public async Task BuscarRelacoes(PessoaDTO pessoa, int coluna, Node box)
		{
			try
			{
				var resultado = PessoaBLL.RealizarConsultaDeRegistrosRelacionados(new RelacaoConsulta(pessoa.Codigo));
				var novaColuna = coluna + 1;			
				foreach (var relacao in resultado)
				{
					CallDeferred("InstanciarRegistroBox", new RegistroObject(relacao, null), ObterColuna(novaColuna), novaColuna);
				}
			}
			catch(Exception ex)
			{
				CallDeferred("ExibirErro", ex.Message);
			}
		}
		public async Task BuscarRelacoes(RegistroDTO registro, int coluna, Node box)
		{
			try
			{
				var resultado = RegistroBLL.RealizarConsultaDeRegistrosRelacionados(new RelacaoConsulta(registro.Codigo));
				var novaColuna = coluna + 1;			
				foreach (var relacao in resultado.Registros)
					CallDeferred("InstanciarRegistroBox", new RegistroObject(relacao, null), ObterColuna(novaColuna), novaColuna);
				foreach (var pessoa in resultado.Pessoas)
					CallDeferred("InstanciarPessoaBox", new PessoaObject(pessoa), ObterColuna(novaColuna), novaColuna);
			}
			catch(Exception ex)
			{
				CallDeferred("ExibirErro", ex.Message);
			}
		}
	}
}
