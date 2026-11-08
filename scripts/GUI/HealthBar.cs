namespace SurvivorsGame
{
    using Godot;

    public partial class HealthBar : ProgressBar
    {
        private Player _player;
        public override void _Ready()
        {
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