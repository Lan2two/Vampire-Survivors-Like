using Godot;
using System;

public partial class Move : States
{
    [Export] public float Speed = 300.0f;
    public override void Enter()
    {
        this.GetPlayer().anim.Play("move");
    }
    public override void PhysicsUpdate(double delta)
    {
        Vector2 velocity = this.GetPlayer().Velocity;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 direction = Input.GetVector("left", "right", "up", "down");
        //flip the sprite based on direction
        if (direction.X != 0)
        {
            this.GetPlayer().anim.FlipH = direction.X < 0;
        }
        this.GetPlayer().Velocity = direction * Speed;
        this.GetPlayer().MoveAndSlide();
        if (direction == Vector2.Zero)
        {
            ChangeState("Idle");
        }
        else if (Input.IsActionPressed("m1"))
        {
            ChangeState("Attack");
        }
    }
}