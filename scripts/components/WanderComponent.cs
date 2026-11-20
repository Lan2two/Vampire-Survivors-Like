using Godot;
using System;

[GlobalClass]
public partial class WanderComponent : Node
{
    [Export] CharacterBody2D characterBody;
    [Export] PathfindComponent pathfindComponent;
    [Export] float WanderSpeed = 10f;
    [Export] float ReturnSpeed = 20f;
    [Export] float WanderRadius = 100f;
    private Vector2 targetPosition;
    private Vector2 originPoint;
    private bool returnHome;

    public override void _Ready()
    {
        if (characterBody == null)
            GD.PrintErr("CharacterBody not set");
        PickNewWanderPoint();
    }

    public void Wander()
    {
        if (characterBody == null || pathfindComponent == null)
            return;
        float distanceToOrigin = characterBody.GlobalPosition.DistanceTo(originPoint);
        if (distanceToOrigin > WanderRadius)
        {
            GD.Print("return home");
            returnHome = true;
            targetPosition = originPoint;
        }
        pathfindComponent.PathfindToPoint(targetPosition);
        if (pathfindComponent.TargetReached())
        {
            if (returnHome)
                returnHome = false;
            PickNewWanderPoint();
        }
    }

    private void PickNewWanderPoint()
    {
        float randAngle = GD.Randf() * Mathf.Tau;
        float randDistance = GD.Randf() * WanderRadius;
        targetPosition = originPoint + new Vector2(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * randDistance;
        GD.Print("New wander point: ", targetPosition);
    }
}
