namespace SurvivorsGame
{
    using Godot;

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
            if (IsInstanceValid(_retryButton))
            {
                _retryButton.Pressed -= RetryButton;
            }
            if (IsInstanceValid(_mainMenuButton))
            {
                _mainMenuButton.Pressed -= MainMenuButton;
            }
        }


        private void RetryButton()
        {
            CallDeferred(nameof(DeferredReloadScene));
        }

        private void DeferredReloadScene()
        {
            if (IsInstanceValid(this) && IsInsideTree())
            {
                GetTree().ReloadCurrentScene();
            }
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