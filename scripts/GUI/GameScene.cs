

namespace SurvivorsGame
{
    using Godot;
    using System;
    public partial class GameScene : Node2D
    {
        Label gameOverLabel;
        CanvasLayer GUI;

        private bool _isChangingScene = false;

        public override void _Ready()
        {
            // Subscribe safely
            Enemy.EnemyDied += CheckforGameOver;

            GUI = GetNodeOrNull<CanvasLayer>("GUI");
            if (GUI != null)
            {
                gameOverLabel = GUI.GetNodeOrNull<Label>("GameOver");
            }
        }

        // Clean up static event subscriptions to avoid infinite memory loops
        public override void _ExitTree()
        {
            Enemy.EnemyDied -= CheckforGameOver;
        }

        private void CheckforGameOver(Enemy enemy)
        {
            if (_isChangingScene || !IsInstanceValid(this)) return;
            if (Enemy.EnemiesKilled >= 20)
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            if (_isChangingScene || !IsInstanceValid(this)) return;
            _isChangingScene = true;

            GetTree().Paused = true;
            if (gameOverLabel != null)
            {
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
                // Explicitly load the start menu scene to pass NativeAOT restrictions
                var menuScene = GD.Load<PackedScene>("res://scenes/start_menu.tscn");
                if (menuScene != null)
                {
                    var menuInstance = menuScene.Instantiate();
                    GetTree().Root.AddChild(menuInstance);
                    GetTree().CurrentScene = menuInstance;
                    QueueFree(); // Free the current game scene safely
                }
                else
                {
                    GD.PrintErr("CRITICAL: Failed to load start_menu.tscn. Check file casing!");
                }
            }
        }
    }
}
