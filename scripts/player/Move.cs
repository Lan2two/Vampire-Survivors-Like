using Godot;
using System;

public partial class Move : States
{
    [Export] public VelocityComponent Velocity;
    public override void Enter()
    {
        this.GetPlayer().anim.Play("move");
    }
    public override void PhysicsUpdate(double delta)
    {

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 direction = Input.GetVector("left", "right", "up", "down");
        Velocity.AccelerateToDirection(direction);
        Velocity.Move(direction);

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