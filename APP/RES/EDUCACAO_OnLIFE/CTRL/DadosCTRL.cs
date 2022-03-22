using Godot;
using System;
using Onlife.CTRL.Interface;


namespace Onlife.CTRL
{
	public class DadosCTRL : Control, IDisposableCTRL
	{
		private AcceptDialog PopErro { get; set; }
		public override void _Ready()
		{
			DesativarFuncoesDeAltoProcessamento();
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
		}
		private void PopularNodes()
		{
			PopErro = GetNode<AcceptDialog>("./Dados/PopErro");
		}
		private void ExibirErro(string mensagem)
		{
			PopErro.DialogText = mensagem;
			PopErro.Popup_();
		}
		public void FecharCTRL()
		{
			PopErro.QueueFree();
			QueueFree();
		}
	}
}
