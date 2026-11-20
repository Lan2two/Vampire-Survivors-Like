using Godot;
using System;

public partial class Idle : States
{
    public override void Enter()
    {
        this.GetPlayer().anim.Play("idle");
    }
    public override void PhysicsUpdate(double delta)
    {
        Vector2 direction = Input.GetVector("left", "right", "up", "down");
        if (direction != Vector2.Zero)
        {
            ChangeState("Move");
        }
    }
}