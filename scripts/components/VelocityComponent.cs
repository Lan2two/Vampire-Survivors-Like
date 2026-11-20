using Godot;
using System;

public partial class VelocityComponent : Node2D
{
    [Export] public CharacterBody2D characterBody;
    [Export] public float maxSpeed = 100.0f;
    [Export] public float acceleration = 40.0f;

    public Vector2 Velocity;


    public override void _Ready()
    {
        base._Ready();
    }

    public void AccelerateToVelocity(Vector2 velocity)
    {
        Velocity = Velocity.Lerp(velocity, acceleration * (float)GetProcessDeltaTime());
    }
    public void AccelerateToDirection(Vector2 direction)
    {
        Velocity = Velocity.Lerp(direction * maxSpeed, acceleration * (float)GetProcessDeltaTime());
    }
    public Vector2 GetMaxVelocity()
    {
        return Velocity * maxSpeed;
    }
    public void Decelerate()
    {
        Velocity = Velocity.Lerp(Vector2.Zero, acceleration * (float)GetProcessDeltaTime());
    }
    public void Move(Vector2 direction)
    {
        if (direction.X != 0)
        {
            this.GetPlayer().anim.FlipH = direction.X < 0;
        }
        characterBody.Velocity = Velocity;
        characterBody.MoveAndSlide();
    }


}
