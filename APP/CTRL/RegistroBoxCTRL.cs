using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using BibliotecaViva.DTO;
using BibliotecaViva.BLL;
using BibliotecaViva.BLL.Utils;
using BibliotecaViva.DTO.Utils;
using BibliotecaViva.BLL.Interface;
using BibliotecaViva.CTRL.Interface;

namespace BibliotecaViva.CTRL
{
	public class RegistroBoxCTRL : Panel, IDisposableCTRL
	{
		private bool Maximizado { get; set; }
		private Label Nome { get; set; }
		private Label Apelido { get; set; }
		
		private RichTextLabel ConteudoDescricao { get; set; }
		private RichTextLabel ConteudoTextual { get; set; }
		private TextureRect ConteudoImagem { get; set; }
		private AudioStreamPlayer ConteudoAudio { get; set; }
		private FileDialog PopupDeDownload { get; set; }
		
		private Control CampoDescricao { get; set; }
		private Control CampoImagem { get; set; }
		private Control CampoTextual { get; set; }
		private Control CampoAudio { get; set; }
		private Control CampoDownload { get; set; }
		
		private RegistroDTO Registro { get; set; }
		private List<TipoDTO> Tipos { get; set; }
		private IConsultarTipoBLL ConsultarTipoBLL { get; set; }
		public override void _Ready()
		{
			RealizarInjecaoDeDependencias();
			PopularNodes();
			DesativarFuncoesDeAltoProcessamento();
		}
		private void RealizarInjecaoDeDependencias()
		{
			ConsultarTipoBLL = new ConsultarTipoBLL();
		}
		private void PopularNodes()
		{
			Maximizado = false;
			Nome = GetNode<Label>("./Nome");
			Apelido = GetNode<Label>("./VBoxContainer/Apelido/Conteudo");
			
			ConteudoDescricao = GetNode<RichTextLabel>("./VBoxContainer/Descricao/ScrollContainer/Conteudo");
			ConteudoTextual = GetNode<RichTextLabel>("./VBoxContainer/Texto/ScrollContainer/Conteudo");
			ConteudoImagem = GetNode<TextureRect>("./VBoxContainer/Imagem/Imagem");
			ConteudoAudio = GetNode<AudioStreamPlayer>("./VBoxContainer/Audio/AudioPlayer");
			PopupDeDownload = GetNode<FileDialog>("./PopupDeDownload");
			
			CampoTextual = GetNode<Control>("./VBoxContainer/Texto");
			CampoImagem = GetNode<Control>("./VBoxContainer/Imagem");
			CampoDescricao = GetNode<Control>("./VBoxContainer/Descricao");
			CampoAudio = GetNode<Control>("./VBoxContainer/Audio");
			CampoDownload = GetNode<Control>("./VBoxContainer/Arquivo");

			Tipos = ConsultarTipoBLL.ConsultarTipos();
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
			SetProcess(false);
		}
		public void Preencher(RegistroDTO registroDTO, Vector2 posicao)
		{
			RectPosition = posicao;
			Registro = registroDTO;
			Nome.Text = registroDTO.Nome;
			PopularCampoOpcional(Apelido, registroDTO.Apelido);
			PopularCampoOpcional(ConteudoDescricao, registroDTO.Descricao);
		}
		private void PopularCampoOpcional(Label campo, string conteudo)
		{
			(campo.GetParent() as Control).Visible = !string.IsNullOrEmpty(conteudo);
			campo.Text = conteudo;
		}
		private void PopularCampoOpcional(RichTextLabel campo, string conteudo)
		{
			(campo.GetParent().GetParent() as Control).Visible = !string.IsNullOrEmpty(conteudo);
			campo.Text = conteudo;
		}
		private void _on_Editar_button_up()
		{
			
		}
		private void _on_Exibir_button_up()
		{
			
		}
		private void _on_Maximizar_button_up()
		{
			if (Maximizado)
				ExibirDescricao();
			else
				ExibirCampo();
			
		}
		private void _on_Play_button_up()
		{
			ConteudoAudio.Play();
		}
		private void _on_Stop_button_up()
		{
			ConteudoAudio.Stop();
		}
		private void _on_Download_button_up()
		{
			PopupDeDownload.CurrentFile = Registro.Nome + ObterDetalhesTipo(Registro.Tipo).Extensao;
			PopupDeDownload.Popup_();
		}
		private void _on_PopupDeDownload_file_selected(String path)
		{
			Task.Run(async () => await SalvarArquivo(path, Registro.Conteudo));
		}
		private async Task SalvarArquivo(string caminho, string base64)
		{
			ImportadorDeBinariosUtil.SalvarBase64(caminho, base64);
		}
		private void ExibirCampo()
		{
			Maximizado = true;
			switch(ObterDetalhesTipo(Registro.Tipo).TipoExecucao)
			{
				case TipoExecucao.Audio:
					ExibirRegistroDeAudio();
					break;
				case TipoExecucao.Imagem:
					ExibirRegistroImagem();
					break;
				case TipoExecucao.Texto:
					ExibirRegistroTextual();
					break;
				case TipoExecucao.Arquivo:
					ExibirRegistroDeArquivo();
					break;
				case TipoExecucao.URL:
					ExibirRegistroURL();
					break;
			}
		}
		private void ExibirDescricao()
		{
			Maximizado = false;
			ConteudoAudio.Stop();
			CampoDescricao.Visible = true;
			CampoImagem.Visible = false;
			CampoTextual.Visible = false;
			CampoAudio.Visible = false;
			CampoDownload.Visible = false;
			RectMinSize = new Vector2(400, 303);
			RectSize = new Vector2(400, 303);
		}
		private void ExibirRegistroDeArquivo()
		{
			CampoDescricao.Visible = false;
			CampoImagem.Visible = false;
			CampoTextual.Visible = false;
			CampoAudio.Visible = false;
			CampoDownload.Visible = true;
			
			RectMinSize = new Vector2(400, 206);
			RectSize = new Vector2(400, 206);
		}
		private void ExibirRegistroTextual()
		{
			CampoTextual.Visible = true;
			CampoDescricao.Visible = false;
			CampoImagem.Visible = false;
			CampoAudio.Visible = false;
			CampoDownload.Visible = false;
			RectMinSize = new Vector2(400, 535);
			RectSize = new Vector2(400, 535);
			
			ConteudoTextual.Text = Registro.Conteudo;
		}
		private void ExibirRegistroDeAudio()
		{
			CampoTextual.Visible = false;
			CampoDescricao.Visible = false;
			CampoImagem.Visible = false;
			CampoAudio.Visible = true;
			CampoDownload.Visible = false;

			var audio = ImportadorDeBinariosUtil.GerarAudio(Registro.Nome, ObterDetalhesTipo(Registro.Tipo).Extensao, Registro.Conteudo);
			ConteudoAudio.Stream = audio;

			RectMinSize = new Vector2(400, 206);
			RectSize = new Vector2(400, 206);
		}
		private void ExibirRegistroImagem()
		{
			CampoImagem.Visible = true;
			CampoDescricao.Visible = false;
			CampoTextual.Visible = false;
			CampoAudio.Visible = false;
			CampoDownload.Visible = false;

			var imagem = ImportadorDeBinariosUtil.GerarImagem(Registro.Nome, ObterDetalhesTipo(Registro.Tipo).Extensao, Registro.Conteudo);
			ConteudoImagem.Texture = imagem;
			RectMinSize = new Vector2(400, 530);
			RectSize = new Vector2(400, 530);
		}
		private void ExibirRegistroURL()
		{
			CampoDescricao.Visible = false;
			CampoImagem.Visible = false;
			CampoTextual.Visible = false;
			CampoAudio.Visible = false;
			CampoDownload.Visible = false;
		}
		public TipoDTO ObterDetalhesTipo(string nomeTipo)
		{
			return (from tipo in Tipos
				where
					tipo.Nome == nomeTipo
				select 
					tipo).FirstOrDefault();
		}
		public void FecharCTRL()
		{
			Nome.QueueFree();
			Apelido.QueueFree();
			PopupDeDownload.QueueFree();
		
			ConteudoDescricao.QueueFree();
			ConteudoTextual.QueueFree();
			ConteudoImagem.QueueFree();
			ConteudoAudio.QueueFree();
			
			CampoDescricao.QueueFree();
			CampoImagem.QueueFree();
			CampoTextual.QueueFree();
			CampoAudio.QueueFree();
			CampoDownload.QueueFree();

			foreach (var tipo in Tipos)
				tipo.Dispose();
			Tipos.Clear();	
			Tipos = null;

			Registro.Dispose();
			ConsultarTipoBLL.Dispose();
			QueueFree();
		}
	}
}
