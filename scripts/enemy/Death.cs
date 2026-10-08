using Godot;
using System;

public partial class Death : Interrupt
{
    [Export] CharacterBody2D characterBody;
    [Export] HitboxComponent hitboxComponent;
    [Export] HealthComponent healthComponent;
    [Export] VelocityComponent velocityComponent;
    [Export] AttackComponent attackComponent;
    [Export] AnimatedSprite2D Sprite;
    public override void _Ready()
    {
        if (Sprite == null)
        {
            GD.PrintErr("Sprite not assigned in DeathComponent.");
        }
        if (healthComponent == null)
        {
            GD.PrintErr("HealthComponent not assigned in DeathComponent.");
        }
        if (hitboxComponent == null)
        {
            GD.PrintErr("HitboxComponent not assigned in DeathComponent.");
        }
        Sprite.AnimationFinished += OnAnimationFinished;
        healthComponent.Damage += CheckforDeath;
    }
    private void CheckforDeath(Attack attackData)
    {
        if (healthComponent.currentHealth <= 0)
        {
            FireInterrupt();
        }
    }

    public override void Enter()
    {
        hitboxComponent.IsInvincible = true;
        attackComponent?.SetDeferred("collision_mask", 0);
        characterBody?.SetDeferred("collision_layer", 0);
        Sprite.Play("die");
    }

    public override void PhysicsUpdate(double delta)
    {
        velocityComponent.Stop();
    }

    public override void _ExitTree()
    {
        Sprite.AnimationFinished -= OnAnimationFinished;
        healthComponent.Damage -= CheckforDeath;
    }
    private void OnAnimationFinished()
    {
        if (Sprite.Animation != "die")
        {
            return;
        }
        characterBody.QueueFree();
    }
}
