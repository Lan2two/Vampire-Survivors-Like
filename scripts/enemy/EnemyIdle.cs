using Godot;
using System;

public partial class EnemyIdle : States
{
    [Export] Enemy enemy;
    [Export] int WanderSpeed = 20;
    [Export] int WanderRadius = 100;
    private Vector2 randomDirection = Vector2.Zero;
    Vector2 targetPosition;
    public override void Enter()
    {
        enemy.anim.Play("idle");
    }

    public override void PhysicsUpdate(double delta)
    {

    }
}