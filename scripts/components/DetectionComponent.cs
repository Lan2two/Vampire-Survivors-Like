using Godot;
using System;

public partial class DetectionComponent : Area2D
{
    [Export] Enemy enemy;
    public override void _Ready()
    {
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Player)
        {

        }
    }
}
