using Godot;
using System;

[GlobalClass]
public partial class VelocityComponent : Node2D
{
    [Export] public CharacterBody2D characterBody;
    [Export] public AnimatedSprite2D animatedSprite2D;
    [Export] public float maxSpeed = 100.0f;
    [Export] public float acceleration = 40.0f;

    public Vector2 Velocity;


    public override void _Ready()
    {
        base._Ready();

        if (characterBody != null)
        {
            characterBody.MotionMode = CharacterBody2D.MotionModeEnum.Floating;
        }
    }

    public void AccelerateToVelocity(Vector2 velocity)
    {
        float step = Mathf.Clamp(acceleration * (float)GetProcessDeltaTime(), 0f, 1f);
        Velocity = Velocity.MoveToward(velocity, step * maxSpeed);
    }

    public void AccelerateToDirection(Vector2 direction)
    {
        float step = Mathf.Clamp(acceleration * (float)GetProcessDeltaTime(), 0f, 1f);
        Velocity = Velocity.MoveToward(direction * maxSpeed, step * maxSpeed);
    }
    public Vector2 GetMaxVelocity()
    {
        return Velocity * maxSpeed;
    }
    public void Decelerate()
    {
        float step = Mathf.Clamp(acceleration * (float)GetProcessDeltaTime(), 0f, 1f);
        Velocity = Velocity.MoveToward(Vector2.Zero, step * maxSpeed);
    }

    public void Stop()
    {
        Velocity = Vector2.Zero;
        if (characterBody != null)
        {
            characterBody.Velocity = Vector2.Zero;
        }
    }


    public void Move(Vector2 direction)
    {
        if (characterBody == null)
            return;

        if (direction.X != 0 && animatedSprite2D != null)
        {
            animatedSprite2D.FlipH = direction.X < 0;
        }

        if (direction == Vector2.Zero)
        {
            Stop();
            return;
        }

        characterBody.Velocity = Velocity;
        characterBody.MoveAndSlide();
    }


}
