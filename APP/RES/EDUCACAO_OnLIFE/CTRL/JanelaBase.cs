using Godot;
using System;

public class JanelaBase : Control
{
	public override void _Ready()
	{
		DesativarFuncoesDeAltoProcessamento();
	}
	private void DesativarFuncoesDeAltoProcessamento()
	{
		SetPhysicsProcess(false);
		SetProcess(false);
	}
	private void _on_CloseButton_pressed()
	{
		QueueFree();
	}
}
