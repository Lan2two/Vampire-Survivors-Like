namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class Stats : ColorRect
    {
        private Label _statsLabel;
        private Player _player;

        public override void _Ready()
        {
            _statsLabel = GetNodeOrNull<Label>("StatsLabel");
            _player = this.GetPlayer();
            if (_statsLabel == null)
            {
                GD.PrintErr("StatsLabel not found in Stats");
                return;
            }
            if (_player == null)
            {
                GD.PrintErr("Player not found in Stats");
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
            _statsLabel.Text = $"Max Health: {(int)_player.healthComponent.maxHealth}\nTotal XP: {(int)_player.totalXP}";
        }

    }
}