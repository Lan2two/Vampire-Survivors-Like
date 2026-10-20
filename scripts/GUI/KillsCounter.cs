namespace SurvivorsGame
{
    using Godot;

    public partial class KillsCounter : Label
    {

        public override void _PhysicsProcess(double delta)
        {
            Text = $"Kills: {Stats.data.EnemiesKilled}";
        }


    }
}
