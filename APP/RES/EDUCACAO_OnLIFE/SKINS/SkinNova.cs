using Godot;
using System;

public class SkinNova : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public void _on_ButtonFoto_pressed()
    {
        GetNode<Control>("Foto").Visible = !GetNode<Control>("Foto").Visible;
    }

    public void _on_ButtonDescricao_pressed()
    {
        GetNode<Control>("Descricao").Visible = !GetNode<Control>("Descricao").Visible;
    }
    public void _on_ButtonDescricao2_pressed()
    {
        GetNode<Control>("Descricao2").Visible = !GetNode<Control>("Descricao2").Visible;
    }
    public void _on_ButtonDescricao3_pressed()
    {
        GetNode<Control>("Descricao3").Visible = !GetNode<Control>("Descricao3").Visible;
    }
}
