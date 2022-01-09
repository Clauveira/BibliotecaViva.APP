using Godot;
using System;

using BLL;
using BLL.Interface;
using CTRL.Interface;

namespace CTRL
{
	public class TabCadastrarRegistroCTRL : Tabs, IDisposableCTRL
	{
		private IConsultarTipoBLL BLLTipo { get; set; }
		private ICadastrarRegistroBLL BLL { get; set; }
		private LineEdit Nome { get; set; }
		private LineEdit Apelido { get; set; }
		private LineEdit LatLong { get ; set; }
		private Label Erro { get; set; }
		private Label Sucesso { get; set; }
		private OptionButton Idioma { get ; set; }
		private OptionButton Tipo { get; set; }
		private TextEdit Descricao { get; set; }
		private TextEdit Conteudo { get; set; }
		public override void _Ready()
		{
			RealizarInjecaoDeDependencias();
			PopularNodes();
			PopularDropDowns();
			DesativarFuncoesDeAltoProcessamento();
		}
		private void RealizarInjecaoDeDependencias()
		{
			BLL = new CadastrarRegistroBLL();
			BLLTipo = new ConsultarTipoBLL();
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
			SetProcess(false);
		}		
		private void PopularNodes()
		{
			Nome = GetNode<LineEdit>("./Inputs/Nome");
			Apelido = GetNode<LineEdit>("./Inputs/Apelido");
			LatLong = GetNode<LineEdit>("./Inputs/LatLong");
			Idioma = GetNode<OptionButton>("./Inputs/Idioma");
			Tipo = GetNode<OptionButton>("./Inputs/Tipo");
			Erro = GetNode<Label>("./Inputs/Erro");
			Sucesso = GetNode<Label>("./Inputs/Sucesso");
			Descricao = GetNode<TextEdit>("./Inputs/Descricao");
			Conteudo = GetNode<TextEdit>("./Inputs/Conteudo");
		}
		private void _on_SalvarAlteracoes_button_up()
		{
			try
			{
				var registro = BLL.PopularRegistro(Nome.Text, Apelido.Text, LatLong.Text, Descricao.Text, Conteudo.Text, Tipo, Idioma);
				LimparPreenchimento();
				Feedback(BLL.CadastrarRegistro(registro), true);
			}
			catch(Exception ex)
			{
				Feedback(ex.Message, false);
			}
		}
		private void Feedback(string mensagem, bool sucesso)
		{
			Sucesso.Text = sucesso ? mensagem : string.Empty;
			Erro.Text = sucesso ? string.Empty : mensagem;
		}
		public void PopularDropDowns()
		{
			BLLTipo.PopularDropDownIdioma(Idioma);
			BLLTipo.PopularDropDownTipo(Tipo);
		}
		private void LimparPreenchimento()
		{
			Nome.Text = string.Empty;
			Apelido.Text = string.Empty; 
			LatLong.Text = string.Empty; 
			Descricao.Text = string.Empty; 
			Conteudo.Text = string.Empty;
		}
		public void FecharCTRL()
		{
			BLL.Dispose();
			Nome.QueueFree();
			Apelido.QueueFree();
			LatLong.QueueFree();
			Idioma.QueueFree();
			Tipo.QueueFree();
			Erro.QueueFree();
			Descricao.QueueFree();
			Conteudo.QueueFree();
			QueueFree();
		}
	}
}
