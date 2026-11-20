using Godot;

[GlobalClass]
public partial class FctComponent : Node2D
{
    [Export] HealthComponent healthComponent;
    private PackedScene damageLabelScene;
    [Export] private double targetDuration = 0.7;
    [Export] private double fadeDuration = 0.6;
    private Tween _fadeTween;
    public override void _Ready()
    {
        if (healthComponent == null)
        {
            GD.PrintErr("HealthComponent not assigned in FctComponent.");
            return;
        }

        if (damageLabelScene == null)
        {
            GD.PrintErr("Damage label scene not assigned in FctComponent.");
            return;
        }

        healthComponent.Damage += DisplayDamage;
        damageLabelScene = GD.Load<PackedScene>("uid://bip5ogwimo5xy");
    }

    public override void _ExitTree()
    {
        if (healthComponent != null)
        {
            healthComponent.Damage -= DisplayDamage;
        }
    }


    private void DisplayDamage(Attack attackData)
    {
        if (damageLabelScene == null || GetTree().CurrentScene == null)
        {
            return;
        }

        FloatingNumbers damageInstance = damageLabelScene.Instantiate<FloatingNumbers>();
        damageInstance.GlobalPosition = GlobalPosition;
        GetTree().CurrentScene.AddChild(damageInstance);
        damageInstance.DisplayDamage(attackData, targetDuration, fadeDuration);
    }
}
