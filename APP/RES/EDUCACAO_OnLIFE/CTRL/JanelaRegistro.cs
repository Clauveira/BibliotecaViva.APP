using Godot;
using System;

namespace Onlife.CTRL
{
	public class JanelaRegistro : MarginContainer
	{
		public override void _Ready()
		{
			DesativarFuncoesDeAltoProcessamento();
		}
		private void PopularNodes()
		{

		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
		}
		public override void _Process(float delta)
		{
			
		}
	}
}