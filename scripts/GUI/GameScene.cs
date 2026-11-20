

namespace SurvivorsGame
{
    using Godot;
    using System;
    public partial class GameScene : Node2D
    {
        Label gameOverLabel;
        CanvasLayer GUI;
        Player player;
        private bool _isChangingScene = false;

        public override void _Ready()
        {
            GUI = GetNodeOrNull<CanvasLayer>("GUI");
            if (GUI != null)
            {
                gameOverLabel = GUI.GetNodeOrNull<Label>("GameOver");
            }
            player = this.GetPlayer();
            Enemy.EnemyDied += CheckForWin;
            player.PlayerDead += PlayerDeath;
        }
        public override void _ExitTree()
        {
            Enemy.EnemyDied -= CheckForWin;
            player.PlayerDead += PlayerDeath;
        }

        private void PlayerDeath()
        {
            GameOver("You died :(");
        }
        private void CheckForWin()
        {
            if (_isChangingScene || !IsInstanceValid(this)) return;
            if (Enemy.EnemiesKilled < 20)
            {
                return;
            }
            GameOver("You Win!!!\nThanks for playing!");
        }

        private void GameOver(string message)
        {
            if (_isChangingScene || !IsInstanceValid(this)) return;
            _isChangingScene = true;

            GetTree().Paused = true;
            if (gameOverLabel != null)
            {
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
