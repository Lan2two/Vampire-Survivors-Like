namespace SurvivorsGame
{
    using Godot;

    public partial class EnemyCounter : Label
    {
        public override void _Ready()
        {
            Text = $"Enemies: {Enemy.Alive}";
        }
        public override void _Process(double delta)
        {
            Text = $"Enemies: {Enemy.Alive}";
        }
    }
}
