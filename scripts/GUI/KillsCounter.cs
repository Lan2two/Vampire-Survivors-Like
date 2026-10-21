namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class KillsCounter : Label
    {
        public override void _Ready()
        {
            Text = $"Kills: {Stats.data.EnemiesKilled}";
            Stats.EnemyDied += UpdateCounter;
        }

        public override void _ExitTree()
        {
            Stats.EnemyDied -= UpdateCounter;
        }

        private void UpdateCounter()
        {
            Text = $"Kills: {Stats.data.EnemiesKilled}";
        }


    }
}
