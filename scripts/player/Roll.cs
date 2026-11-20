using Godot;
using System;

public partial class Roll : States
{
    [Export] HitboxComponent hitboxComponent;
    [Export] VelocityComponent velocityComponent;
    private bool animationplaying;
    Vector2 direction;

    public override void Enter()
    {
        animationplaying = true;
        this.GetPlayer().anim.Play("roll");
        this.GetPlayer().anim.AnimationFinished += OnAnimationFinished;
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
        if (animationplaying)
        {
            velocityComponent.AccelerateToDirection(direction);
            velocityComponent.Move(direction);
        }
    }


    private void OnAnimationFinished()
    {
        animationplaying = false;
        ChangeState("Idle");
    }
}
