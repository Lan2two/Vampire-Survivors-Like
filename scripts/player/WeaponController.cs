using Godot;
using System;

public partial class WeaponController : Node2D
{
    [Export] StateMachine stateMachine;
    Marker2D handposition;
    public override void _Ready()
    {
        handposition = GetNode<Marker2D>("HandPosition");
    }
    public override void _Process(double delta)
    {
        Vector2 mousePosition = GetGlobalMousePosition();
        AimRotationCursor(mousePosition);
    }

    private void AimRotationCursor(Vector2 mousePosition)
    {
        handposition.LookAt(mousePosition);
        Vector2 lookDirection = mousePosition - GlobalPosition;
    }

}
