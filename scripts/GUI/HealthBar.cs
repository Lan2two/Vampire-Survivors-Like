namespace SurvivorsGame
{
    using Godot;

    public partial class HealthBar : ProgressBar
    {
        private Label _healthLabel;
        private Player _player;
        public override void _Ready()
        {
            _healthLabel = GetNodeOrNull<Label>("HealthLabel");
            if (_healthLabel == null)
            {
                GD.PrintErr("HealthLabel not found in HealthBar");
            }
            _player = this.GetPlayer();
            Value = _player.healthComponent.currentHealth;
            MaxValue = _player.healthComponent.maxHealth;
        }
        public override void _PhysicsProcess(double delta)
        {
            Value = _player.healthComponent.currentHealth;
            MaxValue = _player.healthComponent.maxHealth;
        }

    }
}