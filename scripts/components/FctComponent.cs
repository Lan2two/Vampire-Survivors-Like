namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class FctComponent : Node2D
    {
        [Export] private HealthComponent healthComponent;

        private PackedScene damageLabelScene;
        [Export] private double targetDuration = 0.7;
        [Export] private double fadeDuration = 0.6;

        public override void _Ready()
        {
            if (healthComponent == null)
            {
                GD.PrintErr("HealthComponent not assigned in FctComponent.");
                return;
            }

            damageLabelScene = GD.Load<PackedScene>("res://scenes/floating_numbers.tscn");
            if (damageLabelScene == null)
            {
                GD.PrintErr("Could not load res://scenes/floating_numbers.tscn.");
                return;
            }

            healthComponent.Damage += DisplayDamage;
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
            if (!GodotObject.IsInstanceValid(this)
                || damageLabelScene == null
                || GetTree().CurrentScene == null)
            {
                return;
            }

            Node instance = damageLabelScene.Instantiate();
            if (instance is not FloatingNumbers damageNumber)
            {
                GD.PrintErr("Floating number scene root does not use the FloatingNumbers script.");
                instance.QueueFree();
                return;
            }

            GetTree().CurrentScene.AddChild(damageNumber);
            damageNumber.GlobalPosition = GlobalPosition;
            damageNumber.DisplayDamage(attackData, targetDuration, fadeDuration);
        }
    }
}
