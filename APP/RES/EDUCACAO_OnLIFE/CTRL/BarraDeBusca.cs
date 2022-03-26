using Godot;
using System.Collections.Generic;

namespace Onlife.CTRL
{
	public class BarraDeBusca : Control
	{
		private List<string> Opcoes { get; set; }
		private OptionButton Dropdown { get; set; }
		private AnimationPlayer Player { get; set; }
		public override void _Ready()
		{
			DesativarFuncoesDeAltoProcessamento();
			PopularAtributos();
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
			SetProcess(false);
		}
		private void PopularAtributos()
		{
			Dropdown = GetNode<OptionButton>("./Panel/OptionButton");
			Player = GetNode<AnimationPlayer>("./AnimationPlayer");
			Opcoes = new List<string>();
			Opcoes.Add("Pista Viva");
			Opcoes.Add("Registro");
			Dropdown.AddItem("Pista Viva");
			Dropdown.AddItem("Registro");
		}
		public string ObterSelecao()
		{
			var selecao = Dropdown.GetSelectedId();
			return Opcoes[selecao];
		}
		public void Exibir(bool exibir)
		{
			if (exibir)
				Player.Play("Exibir");
			else
				Player.Play("Ocultar");
		}
	}
}
