using Godot;
using System;
using System.Numerics;

[GlobalClass]
public partial class DetectionComponent : Area2D
{
    private bool playerInRange = false;

    public override void _Ready()
    {
        Connect(Area2D.SignalName.BodyEntered, new Callable(this, nameof(OnBodyEntered)));
        Connect(Area2D.SignalName.BodyExited, new Callable(this, nameof(OnBodyExit)));
    }

    public bool IsPlayerInRange()
    {
        return playerInRange;
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Player)
            playerInRange = true;
    }

    private void OnBodyExit(Node body)
    {
        if (body is Player)
            playerInRange = false;
    }
}
