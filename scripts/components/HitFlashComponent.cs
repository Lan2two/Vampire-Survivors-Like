using Godot;
using System;

[GlobalClass]
public partial class HitFlashComponent : Node
{
    [Export] AnimatedSprite2D Sprite;
    [Export] HealthComponent healthComponent;
    [Export] Color flashColor = new Color(1, 0, 0); // Red flash color
    [Export] float flashDuration = 0.1f; // Duration of the flash effect
    private Tween _flashTween;
    public override void _Ready()
    {
        if (Sprite == null)
        {
            GD.PrintErr("Sprite not assigned in HitFlashComponent.");
        }
        if (healthComponent == null)
        {
            GD.PrintErr("HealthComponent not assigned in HitFlashComponent.");
        }
        healthComponent.Damage += Flash;
    }

    public override void _ExitTree()
    {
        healthComponent.Damage -= Flash;
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
}
