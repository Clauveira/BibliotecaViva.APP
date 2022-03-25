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

namespace Onlife.CTRL
{
	public class JanelaRegistro : MarginContainer, IDisposableCTRL
	{
		public int CodigoRegistro { get; set; }
		private bool Maximizado { get; set; }
		private bool EmEdicao { get; set; }
		private TipoExecucao TipoAtual { get; set; }
		private IConsultarTipoBLL BLLTipo { get; set; }
		private ICadastrarRegistroBLL CadastrarRegistroBLL { get; set; }
		private IConsultarRegistroBLL ConsultarRegistroBLL { get; set; }
		private Control Janela { get; set; }
		private VBoxContainer Container { get; set; }
		private VBoxContainer ConteudoTXTContainer { get; set; }
		private VBoxContainer ConteudoAUDIOContainer { get; set; }
		private VBoxContainer ConteudoURLContainer { get; set; }
		private VBoxContainer ConteudoBINContainer { get; set; }
		private VBoxContainer ConteudoIMGContainer { get; set; }
		private VBoxContainer ConteudoTipoContainer { get; set; }
		private LineEdit Nome { get; set; }
		private LineEdit Apelido { get; set; }
		private LineEdit Localizacao { get; set; }
		private LineEdit Descricao { get; set; }
		private LineEdit ConteudoTXT { get; set; }
		private LineEdit URL { get; set; }
		private TextureButton IMG { get; set; }
		private Button BtnEditar { get; set; }
		private Button BtnMaximizar { get; set; }
		private Button BtnAlterarAudio { get; set; }
		private Button BtnAlterarBinario { get; set; }
		private AudioStreamPlayer AudioPlayer { get; set; }
		private FileDialog PopupDeArquivo { get; set; }
		private AcceptDialog PopupDeFeedback { get; set; }
		public override void _Ready()
		{
			PopularNodes();
			RealizarInjecaoDeDependencias();
			DesativarFuncoesDeAltoProcessamento();
			(GetParent() as JanelaBase).PopularConteudo(this);
			AplicarMaximizar();
			AlternarEdicao(false, false);
		}
		public void FecharCTRL()
		{
			BLLTipo.Dispose();
			ConsultarRegistroBLL.Dispose();
			CadastrarRegistroBLL.Dispose();

			ConteudoTXTContainer.QueueFree();
			ConteudoAUDIOContainer.QueueFree();
			ConteudoURLContainer.QueueFree();
			ConteudoBINContainer.QueueFree();
			ConteudoIMGContainer.QueueFree();
			ConteudoTipoContainer.QueueFree();
			BtnMaximizar.QueueFree();
			BtnEditar.QueueFree();
			BtnAlterarAudio.QueueFree();
			BtnAlterarBinario.QueueFree();
			Nome.QueueFree();
			Apelido.QueueFree();
			Localizacao.QueueFree();
			Descricao.QueueFree();
			ConteudoTXT.QueueFree();
			URL.QueueFree();
			IMG.QueueFree();
			AudioPlayer.QueueFree();
			PopupDeArquivo.QueueFree();
			PopupDeFeedback.QueueFree();
		}
		private void RealizarInjecaoDeDependencias()
		{
			BLLTipo = new ConsultarTipoBLL();
			CadastrarRegistroBLL = new CadastrarRegistroBLL();
			ConsultarRegistroBLL = new ConsultarRegistroBLL();
		}
		private void PopularNodes()
		{
			Container = GetNode<VBoxContainer>("./VBoxContainer");
			Janela = GetNode<Control>("../Control");
			ConteudoTXTContainer = GetNode<VBoxContainer>("./VBoxContainer/ConteudoTXT");
			ConteudoAUDIOContainer = GetNode<VBoxContainer>("./VBoxContainer/ConteudoAUDIO");
			ConteudoURLContainer = GetNode<VBoxContainer>("./VBoxContainer/ConteudoURL");
			ConteudoBINContainer = GetNode<VBoxContainer>("./VBoxContainer/ConteudoBIN");
			ConteudoIMGContainer = GetNode<VBoxContainer>("./VBoxContainer/ConteudoIMG");
			ConteudoTipoContainer = GetNode<VBoxContainer>("./VBoxContainer/ConteudoTipo");
			BtnEditar = GetNode<Button>("./VBoxContainer/Comandos/BtnEditar");
			BtnMaximizar = GetNode<Button>("./VBoxContainer/Comandos/BtnExibir");
			BtnAlterarAudio = GetNode<Button>("./VBoxContainer/ConteudoAUDIO/Controles/BtnAlterar");
			BtnAlterarBinario = GetNode<Button>("./VBoxContainer/ConteudoBIN/Controles/BtnAlterar");
			Nome = GetNode<LineEdit>("./VBoxContainer/Nome/LineEdit");
			Apelido = GetNode<LineEdit>("./VBoxContainer/Apelido/LineEdit");
			Localizacao = GetNode<LineEdit>("./VBoxContainer/Localizacao/LineEdit");
			Descricao = GetNode<LineEdit>("./VBoxContainer/Descricao/LineEdit");
			ConteudoTXT = GetNode<LineEdit>("./VBoxContainer/ConteudoTXT/LineEdit");
			URL = GetNode<LineEdit>("./VBoxContainer/ConteudoURL/LineEdit");
			IMG = GetNode<TextureButton>("./VBoxContainer/ConteudoIMG/Borda/Imagem");
			AudioPlayer = GetNode<AudioStreamPlayer>("./AudioPlayer");
			PopupDeArquivo = GetNode<FileDialog>("./FileDialog");
			PopupDeFeedback = GetNode<AcceptDialog>("./Atencao");
			Maximizado = false;
			EmEdicao = false;
			TipoAtual = TipoExecucao.Texto;
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
		}
		public override void _Process(float delta)
		{
			AjustarAltura();
		}
		private void _on_BtnEditar_button_up()
		{
			AlternarEdicao(!EmEdicao, true);
		}
		private void _on_BtnExibir_button_up()
		{
			AlterarMaximizar();
		}
		private void _on_BtnConexoes_button_up()
		{
			// Replace with function body.
		}
		private void _on_OptionButton_item_selected(int index)
		{
			// Replace with function body.
		}
		private void _on_Imagem_button_up()
		{
			// Replace with function body.
		}
		private void _on_BtnDOWNLOAD_button_up()
		{
			// Replace with function body.
		}
		private void _on_BtnAcessar_button_up()
		{
			// Replace with function body.
		}
		private void _on_BtnPlay_button_up()
		{
			AudioPlayer.Play();
		}
		private void _on_BtnStop_button_up()
		{
			AudioPlayer.Stop();
		}
		private void _on_BtnAlterar_button_up()
		{
			// Replace with function body.
		}
		private void _on_FileDialog_file_selected(String path)
		{
			// Replace with function body.
		}
		private void AjustarAltura()
		{
			Janela.RectMinSize = new Vector2(500, Container.RectSize.y + 40);
		}
		private void AlterarMaximizar()
		{
			Maximizado = !Maximizado;
			BtnMaximizar.Text = Maximizado ? "RESUMIR" : "EXIBIR";
			AplicarMaximizar();
		}
		private void AplicarMaximizar()
		{
			AudioPlayer.Stop();
			if (Maximizado)
				ExibirCampo();
			else
			{
				AlternarEdicao(false , false);
				OcultarTodos();
			}

		}
		private void ExibirCampo()
		{
			Maximizado = true;
			switch(TipoAtual)
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
		private void ExibirRegistroDeAudio()
		{
			ConteudoTXTContainer.Hide();
			ConteudoAUDIOContainer.Show();
			ConteudoURLContainer.Hide();
			ConteudoBINContainer.Hide();
			ConteudoIMGContainer.Hide();
			BtnEditar.Show();
		}
		private void ExibirRegistroImagem()
		{
			ConteudoTXTContainer.Hide();
			ConteudoAUDIOContainer.Hide();
			ConteudoURLContainer.Hide();
			ConteudoBINContainer.Hide();
			ConteudoIMGContainer.Show();
			BtnEditar.Show();
		}
		private void ExibirRegistroTextual()
		{
			ConteudoTXTContainer.Show();
			ConteudoAUDIOContainer.Hide();
			ConteudoURLContainer.Hide();
			ConteudoBINContainer.Hide();
			ConteudoIMGContainer.Hide();
			BtnEditar.Show();
		}
		private void ExibirRegistroDeArquivo()
		{
			ConteudoTXTContainer.Hide();
			ConteudoAUDIOContainer.Hide();
			ConteudoURLContainer.Hide();
			ConteudoBINContainer.Show();
			ConteudoIMGContainer.Hide();
			BtnEditar.Show();
		}
		private void ExibirRegistroURL()
		{
			ConteudoTXTContainer.Hide();
			ConteudoAUDIOContainer.Hide();
			ConteudoURLContainer.Show();
			ConteudoBINContainer.Hide();
			ConteudoIMGContainer.Hide();
			BtnEditar.Show();
		}
		private void OcultarTodos()
		{
			AudioPlayer.Stop();
			ConteudoTXTContainer.Hide();
			ConteudoAUDIOContainer.Hide();
			ConteudoURLContainer.Hide();
			ConteudoBINContainer.Hide();
			ConteudoIMGContainer.Hide();
			BtnEditar.Hide();
		}
		private void AlternarEdicao(bool edicao, bool salvar)
		{
			AudioPlayer.Stop();
			AtivarComandos(edicao);
			BtnEditar.Text = EmEdicao ? "SALVAR" : "EDITAR";
			if (EmEdicao)	
			{
				BtnAlterarAudio.Show();
				BtnAlterarBinario.Show();
				ConteudoTipoContainer.Show();
			}
			else
			{
				BtnAlterarAudio.Hide();
				BtnAlterarBinario.Hide();
				ConteudoTipoContainer.Hide();
			}
		}
		private void AtivarComandos(bool ativar)
		{
			EmEdicao = ativar;
			Nome.Editable = ativar;
			Apelido.Editable = ativar;
			Localizacao.Editable = ativar;
			Descricao.Editable = ativar;
			ConteudoTXT.Editable = ativar;;
			URL.Editable = ativar;
			IMG.Disabled = !ativar;
		}
	}
}
