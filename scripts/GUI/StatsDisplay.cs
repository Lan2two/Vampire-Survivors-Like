namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class StatsDisplay : Panel
    {
        private Label _statsLabel;
        private Player _player;

        public override void _Ready()
        {
            _statsLabel = GetNodeOrNull<Label>("%StatsLabel");
            _player = this.GetPlayer();
            if (_statsLabel == null)
            {
                GD.PrintErr("StatsLabel not found in StatsDisplay");
                return;
            }
            if (_player == null)
            {
                GD.PrintErr("Player not found in StatsDisplay");
                return;
            }
            GameScene.GameOverEvent += OnGameOver;
        }

        public override void _ExitTree()
        {
            GameScene.GameOverEvent -= OnGameOver;
        }

        private void OnGameOver()
        {
            TimeSpan time = TimeSpan.FromSeconds(Stats.data.Seconds);
            string[] stats =
            [
                $"Time Elapsed: {time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}",
                $"Coins Collected: {Stats.data.Coins}",
                $"Enemies Killed: {Stats.data.EnemiesKilled}",
                $"Level: {Stats.data.Level}",
                $"XP Collected: {(int)Stats.data.totalXP}",
                $"Damage Dealt: {(int)Stats.data.DamageDealt}",
                $"Damage Taken: {(int)Stats.data.DamageTaken}",
                $"Health Healed: {(int)Stats.data.HealthHealed}",
                $"Max Health: {(int)_player.healthComponent.maxHealth}",
                $"Max Speed: {(int)_player.velocityComponent.maxSpeed}",
                $"Campfires Lit: {Stats.data.CampfiresLit}"
            ];
            _statsLabel.Text = string.Join(System.Environment.NewLine, stats);
        }

    }
}