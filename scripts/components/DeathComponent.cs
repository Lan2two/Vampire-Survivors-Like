using Godot;
using System;

[GlobalClass]
public partial class DeathComponent : Node
{
    [Export] CharacterBody2D characterBody;
    [Export] HealthComponent healthComponent;
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
        if (characterBody != null)
        {
            characterBody.SetDeferred("collision_layer", 0);
            characterBody.SetDeferred("collision_mask", 0);
        }
        if (Sprite.SpriteFrames.HasAnimation("die"))
        {
            Sprite.Play("die");
            await ToSignal(Sprite, AnimatedSprite2D.SignalName.AnimationFinished);
        }
        GetParent().QueueFree();
    }
}
