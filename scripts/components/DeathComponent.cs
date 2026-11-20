using Godot;
using System;

[GlobalClass]
public partial class DeathComponent : Node
{
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
        healthComponent.Damage += Die;
    }
    public override void _ExitTree()
    {
        healthComponent.Damage -= Die;
    }

    private async void Die()
    {
        if (Sprite.SpriteFrames.HasAnimation("die"))
        {
            Sprite.Play("die");
            await ToSignal(Sprite, AnimatedSprite2D.SignalName.AnimationFinished);
            GetParent().QueueFree();
        }
        else
        {
            GetParent().QueueFree();
        }
    }
}
