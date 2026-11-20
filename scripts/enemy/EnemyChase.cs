using Godot;
using System;

public partial class EnemyChase : States
{
    [Export] Enemy enemy;
    [Export] DetectionComponent detectionComponent;
    [Export] PathfindComponent pathfindComponent;

    public override void Enter()
    {
        enemy.anim.Play("idle");
    }
    public override void PhysicsUpdate(double delta)
    {
        if (detectionComponent?.IsPlayerInRange() == true)
        {
            enemy.anim.Play("chase");
        }
        pathfindComponent?.PathfindToPlayer();
    }

}
