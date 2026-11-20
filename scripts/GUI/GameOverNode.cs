namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class GameOverNode : Control
    {
        private Button _retryButton;
        private Button _mainMenuButton;

        public override void _Ready()
        {
            _retryButton = GetNodeOrNull<Button>("GameOverContainer/RetryButton");
            _mainMenuButton = GetNodeOrNull<Button>("GameOverContainer/MainMenuButton");
            if (_retryButton == null)
            {
                GD.PrintErr("RetryButton not found in GameOverNode");
                return;
            }
            if (_mainMenuButton == null)
            {
                GD.PrintErr("MainMenuButton not found in GameOverNode");
                return;
            }
            _retryButton.Pressed += RetryButton;
            _mainMenuButton.Pressed += MainMenuButton;
        }

        public override void _ExitTree()
        {
            if (_retryButton != null)
            {
                _retryButton.Pressed -= RetryButton;
            }
            if (_mainMenuButton != null)
            {
                _mainMenuButton.Pressed -= MainMenuButton;
            }
        }


        private void RetryButton()
        {
            GetTree().ReloadCurrentScene();
        }

        private void MainMenuButton()
        {
            CallDeferred(nameof(DeferredChangeScene));
        }
        private void DeferredChangeScene()
        {
            if (IsInstanceValid(this) && IsInsideTree())
            {
                GetTree().Paused = false;
                GetTree().ChangeSceneToFile("res://scenes/start_menu.tscn");
            }
        }
    }
}