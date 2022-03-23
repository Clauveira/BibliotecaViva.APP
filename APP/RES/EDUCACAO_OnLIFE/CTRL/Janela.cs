using Godot;
using System;
using RandomGen;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using BibliotecaViva.DTO;
using BibliotecaViva.BLL;
using BibliotecaViva.DTO.Utils;
using BibliotecaViva.DTO.Dominio;
using BibliotecaViva.BLL.Interface;
using BibliotecaViva.CTRL.Interface;

namespace Onlife.CTRL
{
	public class Janela : Control
	{
		public int Coluna { get; set; }
		public int CodPessoa { get; set; }
		public PessoaDTO Pessoa { get; set; }
		public RegistroDTO FotodDTO { get; set; }
		public RegistroDTO LattesDTO { get; set; }
		public RegistroDTO ResearchGateDTO { get; set; }
		public RegistroDTO IDDTO { get; set; }
		private LineEdit Nome { get; set; }
		private LineEdit Sobrenome { get; set; }
		private LineEdit Apelido { get; set; }
		private Control NodoJanelas { get; set; } = null;
		private Button Editar { get ;set; }
		private TextureButton Foto { get; set; }
		private string Titulo = "Nova Pista Viva";
		private bool EmEdicao { get; set; }
		private ICadastrarPessoaBLL CadastroPessoaBLL { get; set; }
		private IConsultarPessoaBLL ConsultarPessoaBLL { get; set; }
		private IConsultarTipoBLL TipoBLL { get; set; }
		private IConsultarRegistroBLL ConsultarRegistroBLL { get; set; }
		private ICadastrarRegistroBLL CadastrarRegistroBLL { get; set; }
		private AcceptDialog PopupFeedback { get; set; }
		private Popup PopupURL { get; set; }
		private Label TituloURL { get; set; }
		private LineEdit URL { get; set; }
		private RegistroDTO RegistroPOP { get; set; }
		private string NomePopUp { get ; set; }
		public override void _Ready()
		{
			//DefinirTitulo();
			PoularNodes();
			RealizarInjecaoDeDependencias();
			EmEdicao = false;
		}
		private void DefinirTitulo()
		{
			set_titulo(Titulo);
			NodoJanelas = GetParent<Control>();
			set_titulo(Titulo);
		}
		private void RealizarInjecaoDeDependencias()
		{
			CadastroPessoaBLL = new CadastrarPessoaBLL();
			ConsultarPessoaBLL = new ConsultarPessoaBLL();
			TipoBLL = new ConsultarTipoBLL();
			ConsultarRegistroBLL = new ConsultarRegistroBLL();
			
		}
		private void PoularNodes()
		{
			CodPessoa = 0;
			Nome = GetNode<LineEdit>("./Conteudo/HBoxContainer/LabelInput/Nome/LineEdit");
			Sobrenome = GetNode<LineEdit>("./Conteudo/HBoxContainer/LabelInput/Sobrenome/LineEdit");
			Apelido = GetNode<LineEdit>("./Conteudo/HBoxContainer/LabelInput/Apelido/LineEdit");
			Editar = GetNode<Button>("./Conteudo/HBoxContainer/LabelInput/HBoxContainer2/BtnEditar");
			Foto = GetNode<TextureButton>("./Conteudo/HBoxContainer/VBoxContainer/MarginContainer/Borda/Foto");

			PopupURL = GetNode<Popup>("./URL");
			PopupFeedback = GetNode<AcceptDialog>("./Atencao");
			TituloURL = GetNode<Label>("./URL/Nome/Label");
			URL = GetNode<LineEdit>("./URL/Nome/LineEdit");
		}
		public override void _Process(float delta)
		{
			
		}
		public void _on_CloseButton_pressed()
		{
			QueueFree();
		}
		public void set_titulo(string novo_titulo)
		{
			GetNode<Label>("BarraDeTituloJanela/MarginContainer/Titulo").Text = Titulo;
		}
		private void AtivarEditacao(bool editar)
		{
			EmEdicao = editar;
			DefinirEmEdicao();
		}
		private void DefinirEmEdicao()
		{
			Editar.Text = EmEdicao? "SALVAR" : "EDITAR";
			Foto.Disabled = !EmEdicao;
			Nome.Editable = EmEdicao;
			Sobrenome.Editable = EmEdicao;
			Apelido.Editable = EmEdicao;
		}
		private async void _on_BtnEditar_button_up()
		{
			if (EmEdicao)
				SalvarDadosPessoa();
			EmEdicao = !EmEdicao;
			DefinirEmEdicao();
		}
		private void SalvarDadosPessoa()
		{
			Task.Run(async () => await RelizarEnvioRegistro());
		}
		public async Task RelizarEnvioRegistro()
		{
			try
			{
				var pessoa = CadastroPessoaBLL.PopularPessoa(Nome.Text, Sobrenome.Text, "Prefiro não Declarar", Apelido.Text, CodPessoa, new List<RelacaoDTO>());
				var retorno = CadastroPessoaBLL.CadastrarPessoa(pessoa);
				CallDeferred("Feedback", retorno, true);
			}
			catch(Exception ex)
			{
				CallDeferred("Feedback", ex.Message, false);
			}
		}
		
		private void _on_Foto_button_up()
		{
			
		}
		private void _on_ID_button_up()
		{
			if (EmEdicao)
				AbirPopupDeAlteracaoURL(IDDTO, "Lattes");
			else
				AbrirURL(IDDTO);
		}
		private void _on_Lattes_button_up()
		{
			if (EmEdicao)
				AbirPopupDeAlteracaoURL(LattesDTO, "Lattes");
			else
				AbrirURL(LattesDTO);
		}
		private void _on_ResearchGate_button_up()
		{
			if (EmEdicao)
				AbirPopupDeAlteracaoURL(ResearchGateDTO, "ResearchGate");
			else
				AbrirURL(ResearchGateDTO);
		}
		private void _on_BtnConexoes_button_up()
		{
			//Task.Run(async () => await TabBuscar.BuscarRelacoes(Pessoa, Coluna, this));
		}
		private void _on_BtnEditarURL_button_up()
		{
			AlterarURL();
		}
		private void AbrirURL(RegistroDTO registro)
		{
			if (registro != null && !string.IsNullOrEmpty(registro.Conteudo))
				OS.ShellOpen(registro.Conteudo);
		}
		private void AbirPopupDeAlteracaoURL(RegistroDTO registro, string nome)
		{
			TituloURL.Text = "Por favor insira a URL do " + nome;
			RegistroPOP = registro;
			NomePopUp = nome;
			
			if (RegistroPOP != null)
				URL.Text = RegistroPOP.Conteudo;
			
			PopupURL.Popup_();
		}
		private void AlterarURL()
		{
			if (RegistroPOP == null)
				RegistroPOP = new RegistroDTO()
				{
					Codigo = 0,
					Nome = GerarNomeAleatorio(NomePopUp),
					Idioma = "Português",
					Tipo = "Link ou URL",
					Conteudo = URL.Text,
					DataInsercao = DateTime.Now,
					Referencias = new List<RelacaoDTO>()
				};
			else
			{
				RegistroPOP.Conteudo = URL.Text;
				RegistroPOP.DataInsercao = DateTime.Now;
			}
		}
		private string GerarNomeAleatorio(string prefixo)
		{
			var contagem = 30 - prefixo.Count();
			return prefixo + Gen.Random.Text.Length(contagem).Invoke();
		}
		private void Feedback(string mensagem, bool sucesso)
		{
			var mensagemFinal = sucesso ? "Retorno: " + mensagem : "Erro: " + mensagem;
			PopupFeedback.DialogText = mensagemFinal;
			PopupFeedback.Popup_();
		}
	}
}
