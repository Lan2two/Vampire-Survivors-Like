namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class StatsDisplay : ColorRect
    {
        private Label _statsLabel;
        private Player _player;

        public override void _Ready()
        {
            _statsLabel = GetNodeOrNull<Label>("StatsLabel");
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
            _statsLabel.Text = $"Enemies Killed: {(int)Stats.data.EnemiesKilled}\nTotal XP: {(int)_player.totalXP}\nDamage Dealt: {(int)Stats.data.DamageDealt}\nDamage Taken: {(int)Stats.data.DamageTaken}\nHealth Healed: {(int)Stats.data.HealthHealed}\nMax Health: {(int)_player.healthComponent.maxHealth}\nMax Speed: {(int)_player.velocityComponent.maxSpeed}\nCampfires Lit: {Stats.data.CampfiresLit}";
        }

    }
}