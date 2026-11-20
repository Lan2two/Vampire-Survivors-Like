using Godot;
using System;

[GlobalClass]
public partial class PathfindComponent : Node
{
    [Export] VelocityComponent velocityComponent;
    Vector2 target;

    public void PathfindToPlayer(CharacterBody2D characterBody)
    {
        Godot.Vector2 target = characterBody.GlobalPosition.DirectionTo(this.GetPlayer()?.GlobalPosition ?? characterBody.GlobalPosition);
        GD.Print(target);
        velocityComponent.AccelerateToDirection(target);
        velocityComponent.Move(target);
    }
}
