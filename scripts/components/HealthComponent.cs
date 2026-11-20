using Godot;
using System;

[GlobalClass]
public partial class HealthComponent : Node
{
    [Export] int maxHealth = 100;
    [Export] AnimatedSprite2D Sprite;
    [Export] Color flashColor = new Color(1, 0, 0); // Red flash color
    [Export] float flashDuration = 0.1f; // Duration of the flash effect


    private Tween _flashTween;
    int currentHealth;

    public override void _Ready()
    {
        if (Sprite == null)
        {
            GD.PrintErr("Sprite not assigned in HealthComponent.");
        }
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Flash();
        GD.Print("Took damage: " + damage + ", Current Health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Flash()
    {
        if (_flashTween != null && _flashTween.IsRunning())
        {
            _flashTween.Kill();
        }
        else
        {
            _flashTween = CreateTween();
            _flashTween.TweenProperty(Sprite, "modulate", flashColor, flashDuration).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
            _flashTween.TweenProperty(Sprite, "modulate", Colors.White, flashDuration).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut).SetDelay(flashDuration);
        }
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
