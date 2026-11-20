using Godot;
using System;

public partial class Idle : States
{
    [Export] Player player;
    public override void Enter()
    {
        player.anim.Play("idle");
    }
    public override void Exit()
    {
    }
    public override void Update(double delta)
    {
    }
    public override void PhysicsUpdate(double delta)
    {
        Vector2 direction = Input.GetVector("left", "right", "up", "down");
        if (direction != Vector2.Zero)
        {
            ChangeState("Move");
        }
        else if (Input.IsActionPressed("m1"))
        {
            ChangeState("Attack");
        }

    }
}