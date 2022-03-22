using Godot;
using System;
using BibliotecaViva.DTO;

namespace Onlife.CTRL
{
	public class Janela : Control
	{
		public int Coluna { get; set; }
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
		public override void _Ready()
		{
			//DefinirTitulo();
			PoularNodes();
			EmEdicao = false;
		}
		private void DefinirTitulo()
		{
			set_titulo(Titulo);
			NodoJanelas = GetParent<Control>();
			set_titulo(Titulo);
		}
		private void PoularNodes()
		{
			Nome = GetNode<LineEdit>("./Conteudo/HBoxContainer/LabelInput/Nome/LineEdit");
			Sobrenome = GetNode<LineEdit>("./Conteudo/HBoxContainer/LabelInput/Sobrenome/LineEdit");
			Apelido = GetNode<LineEdit>("./Conteudo/HBoxContainer/LabelInput/Apelido/LineEdit");
			Editar = GetNode<Button>("./Conteudo/HBoxContainer/LabelInput/HBoxContainer2/BtnEditar");
			Foto = GetNode<TextureButton>("./Conteudo/HBoxContainer/VBoxContainer/MarginContainer/Borda/Foto");
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
		private void _on_BtnEditar_button_up()
		{
			EmEdicao = !EmEdicao;
			DefinirEmEdicao();
		}
		private void _on_Foto_button_up()
		{
			// Replace with function body.
		}
		private void _on_ID_button_up()
		{
			// Replace with function body.
		}
		private void _on_Lattes_button_up()
		{
			// Replace with function body.
		}
		private void _on_ResearchGate_button_up()
		{
			// Replace with function body.
		}
		private void _on_BtnConexoes_button_up()
		{
			// Replace with function body.
		}
	}
}
