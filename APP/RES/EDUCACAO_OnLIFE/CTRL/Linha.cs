using Godot;
using System;

public class Linha : Line2D
{
    [Export]
    public NodePath ponto_a = null;
    [Export]
    public NodePath ponto_b = null;

    public Godot.Object ponto_a_obj;
    public Godot.Object ponto_b_obj;

    public override void _Ready()
    {
        SetPointPosition(0, new Vector2(0, 0));
        SetPointPosition(1, new Vector2(-200, 150));
        ponto_a_obj = GetNode(ponto_a);
        ponto_b_obj = GetNode(ponto_b);
        Atualizar_posicao();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        Atualizar_posicao();
    }

    public void Atualizar_posicao()
    {
        if (IsInstanceValid(ponto_a_obj) == false || IsInstanceValid(ponto_b_obj) == false)
        {
            QueueFree();
        }
        else if (ponto_a != null && ponto_b != null)
        {
            //GD.Print("Reposiciona Linha!");

            SetPointPosition(0, GetNode<Position2D>(ponto_a).GlobalPosition);
            SetPointPosition(1, GetNode<Position2D>(ponto_b).GlobalPosition);
        }
        else
        {
            GD.Print("null!");
        }
    }

}
