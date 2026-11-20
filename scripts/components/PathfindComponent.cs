using Godot;
using System;

[GlobalClass]
public partial class PathfindComponent : Node
{
    [Export] CharacterBody2D characterBody;
    [Export] VelocityComponent velocityComponent;
    private float distanceToTarget;
    private bool targetReached = false;
    private float threshold = 3.0f;

    public void PathfindToPlayer()
    {
        if (characterBody == null || velocityComponent == null)
            return;

        Player player = this.GetPlayer();
        if (player == null)
            return;

        Vector2 target = characterBody.GlobalPosition.DirectionTo(player.GlobalPosition).Normalized();
        distanceToTarget = characterBody.GlobalPosition.DistanceTo(player.GlobalPosition);
        targetReached = distanceToTarget <= threshold;

        if (!targetReached)
        {
            velocityComponent.AccelerateToDirection(target);
            velocityComponent.Move(target);
            return;
        }

        velocityComponent.Stop();
    }

    public void PathfindToPoint(Vector2 target)
    {
        targetReached = false;
        Vector2 direction = characterBody.GlobalPosition.DirectionTo(target).Normalized();
        distanceToTarget = characterBody.GlobalPosition.DistanceTo(target);
        if (distanceToTarget >= threshold)
        {
            velocityComponent.AccelerateToDirection(direction);
            velocityComponent.Move(direction);
        }
        else
        {
            targetReached = true;
        }
    }

    public float DistanceFromTarget()
    {
        return distanceToTarget;
    }

    public bool TargetReached()
    {
        return targetReached;
    }
}
