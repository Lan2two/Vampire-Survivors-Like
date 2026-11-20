using Godot;
using System;

public partial class Roll : States
{
    [Export] HitboxComponent hitboxComponent;
    [Export] VelocityComponent velocityComponent;
    [Export] double rollCooldown = 5f;
    private bool animationplaying;
    private double timer = 0;
    Vector2 direction;

    public override void Enter()
    {
        this.GetPlayer().anim.AnimationFinished += OnAnimationFinished;
        if (timer > 0)
        {
            ChangeState("Move");
            return;
        }
        animationplaying = true;
        this.GetPlayer().anim.Play("roll");
        hitboxComponent.SetDeferred("monitorable", false);
        direction = Input.GetVector("left", "right", "up", "down");
    }

    public override void Exit()
    {
        this.GetPlayer().anim.AnimationFinished -= OnAnimationFinished;
        hitboxComponent.SetDeferred("monitorable", true);
    }

    public override void PhysicsUpdate(double delta)
    {
        if (timer > 0)
        {
            return;
        }
        if (animationplaying)
        {
            velocityComponent.AccelerateToDirection(direction);
            velocityComponent.Move(direction);
        }
    }


    private void OnAnimationFinished()
    {
        timer = rollCooldown;
        animationplaying = false;
        ChangeState("Idle");
    }

    public override void _PhysicsProcess(double delta)
    {
        timer -= delta;
    }

}
