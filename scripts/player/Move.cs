using Godot;
using System;

public partial class Move : States
{
    [Export] Player player;
    [Export] public float Speed = 300.0f;
    public override void Enter()
    {
        player.anim.Play("move");
    }
    public override void PhysicsUpdate(double delta)
    {
        Vector2 velocity = player.Velocity;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 direction = Input.GetVector("left", "right", "up", "down");
        //flip the sprite based on direction
        if (direction.X != 0)
        {
            player.anim.FlipH = direction.X < 0;
        }
        player.Velocity = direction * Speed;
        player.MoveAndSlide();
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