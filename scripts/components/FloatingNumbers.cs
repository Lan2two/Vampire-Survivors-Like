using Godot;

public partial class FloatingNumbers : Node2D
{
    private Label _damageLabel;
    private Tween _fadeTween;
    public void DisplayDamage(Attack attackData, double targetDuration, double fadeDuration)
    {
        _damageLabel = GetNodeOrNull<Label>("DamageLabel");

        _damageLabel.Text = attackData.Damage.ToString();
        _fadeTween = CreateTween().SetParallel(true);
        Vector2 targetPosition = GlobalPosition + new Vector2(GD.RandRange(-30, 30), -35);
        _fadeTween.TweenProperty(this, "global_position", targetPosition, targetDuration).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
        _fadeTween.TweenProperty(this, "modulate:a", 0.0, fadeDuration).SetDelay(0.3);
        _fadeTween.Chain().TweenCallback(Callable.From(QueueFree));
    }
}
