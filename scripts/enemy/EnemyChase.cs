using Godot;
using System;

public partial class EnemyChase : States
{
    [Export] Enemy enemy;
    [Export] DetectionComponent detectionComponent;
    [Export] PathfindComponent pathfindComponent;

    public override void Enter()
    {
        enemy.anim.Play("chase");
    }
    public override void PhysicsUpdate(double delta)
    {
        if (!detectionComponent.IsPlayerInRange())
        {
            ChangeState("idle");
        }
        pathfindComponent.PathfindToPlayer(enemy);
    }

}
