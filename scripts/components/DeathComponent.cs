using Godot;
using System;

[GlobalClass]
public partial class DeathComponent : Node
{
    [Export] CharacterBody2D characterBody;
    [Export] HealthComponent healthComponent;
    [Export] VelocityComponent velocityComponent;
    [Export] DamageComponent damageComponent;
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
    }
    public override void _PhysicsProcess(double delta)
    {
        if (healthComponent.currentHealth <= 0)
            Die();
    }

    private async void Die()
    {
        damageComponent?.QueueFree();
        velocityComponent?.Stop();
        characterBody?.QueueFree();

        if (Sprite?.SpriteFrames.HasAnimation("die") == true)
        {
            Sprite.Play("die");
            await ToSignal(Sprite, AnimatedSprite2D.SignalName.AnimationFinished);
        }
        GetParent().QueueFree();
    }
}
