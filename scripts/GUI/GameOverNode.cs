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
            if (!IsInsideTree())
            {
                return;
            }

            SaveSystem saveSystem = GetSaveSystem();
            if (saveSystem != null)
            {
                saveSystem.ReloadCurrentSceneDeferred();
            }
        }

        private void MainMenuButton()
        {
            if (!IsInsideTree())
            {
                return;
            }

            SaveSystem saveSystem = GetSaveSystem();
            if (saveSystem != null)
            {
                saveSystem.ChangeSceneToFileDeferred("res://scenes/start_menu.tscn");
            }
        }

        private SaveSystem GetSaveSystem()
        {
            SaveSystem saveSystem = GetNodeOrNull<SaveSystem>("/root/SaveSystem");
            if (saveSystem == null)
            {
                GD.PrintErr("SaveSystem autoload not found.");
            }

            return saveSystem;
        }
    }
}