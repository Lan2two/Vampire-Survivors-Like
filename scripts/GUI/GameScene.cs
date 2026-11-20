

namespace SurvivorsGame
{
    using Godot;
    using System;
    public partial class GameScene : Node2D
    {
        Label gameOverLabel;
        CanvasLayer GUI;
        Player player;
        ColorRect Dim;
        private bool _isChangingScene = false;

        public override void _Ready()
        {
            GUI = GetNodeOrNull<CanvasLayer>("GUI");
            if (GUI != null)
            {
                gameOverLabel = GUI.GetNodeOrNull<Label>("GameOver");
                Dim = GUI.GetNodeOrNull<ColorRect>("Dim");
            }
            if (GUI == null)
            {
                GD.PrintErr("GUI not found in GameScene");
                return;
            }
            if (gameOverLabel == null)
            {
                GD.PrintErr("gameOverLabel not found in GUI");
                return;
            }
            if (Dim == null)
            {
                GD.PrintErr("Dim not found in GUI");
                return;
            }
            player = this.GetPlayer();
            player.PlayerDead += PlayerDeath;
        }
        public override void _ExitTree()
        {
            player.PlayerDead += PlayerDeath;
        }

        private void PlayerDeath()
        {
            GameOver("You died :(");
        }

        private void GameOver(string message)
        {
            if (_isChangingScene || !IsInstanceValid(this)) return;
            _isChangingScene = true;
            GetTree().Paused = true;
            if (gameOverLabel != null)
            {
                Dim.Visible = true;
                gameOverLabel.Text = message;
                gameOverLabel.Visible = true;
            }

            GetTree().CreateTimer(5).Timeout += ReturnToMenu;
        }

        private void ReturnToMenu()
        {
            Enemy.EnemiesKilled = 0;
            GetTree().Paused = false;
            CallDeferred(nameof(DeferredChangeScene));
        }

        private void DeferredChangeScene()
        {
            if (IsInstanceValid(this) && IsInsideTree())
            {
                GetTree().ChangeSceneToFile("res://scenes/start_menu.tscn");
            }
        }
    }
}
