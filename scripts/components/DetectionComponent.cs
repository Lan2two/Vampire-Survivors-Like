using Godot;
using System;

[GlobalClass]
public partial class DetectionComponent : Area2D
{
    [Export] CharacterBody2D characterBody;
    [Export] VelocityComponent Velocity;
    Vector2 target;

    public override void _Ready()
    {
        Connect(Area2D.SignalName.BodyEntered, new Callable(this, nameof(OnBodyEntered)));
        Connect(Area2D.SignalName.BodyExited, new Callable(this, nameof(OnBodyExit)));
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Player)
        {
            target = GlobalPosition.DirectionTo(this.GetPlayer()?.GlobalPosition ?? GlobalPosition);
            GD.Print(target);
            Velocity.AccelerateToDirection(target);
            Velocity.Move(target);

        }
    }

    private void OnBodyExit(Node body)
    {

    }
}
