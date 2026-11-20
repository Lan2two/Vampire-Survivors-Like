using Godot;
using System;

public partial class EnemyIdle : States
{
    [Export] Enemy enemy;

    [Export] DetectionComponent detectionComponent;
    public override void Enter()
    {
        enemy.anim.Play("idle");
    }

    public override void PhysicsUpdate(double delta)
    {
        if (detectionComponent.IsPlayerInRange())
        {
            ChangeState("Chase");
        }
    }
}