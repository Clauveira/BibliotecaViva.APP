using Godot;
using System;

namespace Onlife.CTRL
{
	public class JanelaRegistro : MarginContainer
	{
		private Control Janela { get; set; }
		private VBoxContainer Container { get; set; }
		public override void _Ready()
		{
			PopularNodes();
			DesativarFuncoesDeAltoProcessamento();
		}
		private void PopularNodes()
		{
			Container = GetNode<VBoxContainer>("./VBoxContainer");
			Janela = GetNode<Control>("../Control");
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
		}
		public override void _Process(float delta)
		{
			AjustarAltura();
		}
		private void AjustarAltura()
		{
			Janela.RectMinSize = new Vector2(500, Container.RectSize.y + 40);
		}
	}
}
