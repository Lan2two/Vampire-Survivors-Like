using Godot;
using System;
using System.Numerics;

[GlobalClass]
public partial class DetectionComponent : Area2D
{
    bool playerInRange;

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
        playerInRange = true;
    }

    private void OnBodyExit(Node body)
    {
        playerInRange = false;
    }
}
