namespace SurvivorsGame
{
    using Godot;
    public partial class HpOrb : Absorbable
    {
        [Export] public float HPAmount = 10f;
        public override void _Ready()
        {
            BodyEntered += OnBodyEntered;
        }

        public override void _ExitTree()
        {
            BodyEntered -= OnBodyEntered;
        }

        private void OnBodyEntered(Node body)
        {
            if (body is Player)
            {
                Player player = (Player)body;
                player.healthComponent.currentHealth += HPAmount;
                Stats.data.HealthHealed += HPAmount;
            }
        }
    }
}