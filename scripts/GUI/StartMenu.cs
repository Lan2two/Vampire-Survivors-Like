using Godot;

namespace SurvivorsGame
{
    public partial class StartMenu : Control
    {
        private Control _menuContainer;
        private Button _startButton;
        private Button _optionsButton;
        private Button _statsButton;
        private Button _exitButton;
        private Fade _fadeRect;
        private AnimationPlayer _menuAnimation;
        private AnimationPlayer _optionAnimations;

        public override void _Ready()
        {
            _fadeRect = GetNodeOrNull<Fade>("Fade");
            _menuContainer = GetNodeOrNull<Control>("MainMenuContainer");
            _startButton = GetNodeOrNull<Button>("MainMenuContainer/StartButton");
            _optionsButton = GetNodeOrNull<Button>("MainMenuContainer/OptionsButton");
            _statsButton = GetNodeOrNull<Button>("MainMenuContainer/StatsButton");
            _exitButton = GetNodeOrNull<Button>("MainMenuContainer/ExitButton");
            _menuAnimation = GetNodeOrNull<AnimationPlayer>("AnimationPlayers/MainMenu");
            _optionAnimations = GetNodeOrNull<AnimationPlayer>("AnimationPlayers/Options");

            if (_menuContainer == null || _startButton == null || _exitButton == null)
            {
                GD.PrintErr("Required UI components missing in StartMenu");
                return;
            }

            _startButton.Pressed += StartButton;
            _optionsButton.Pressed += OptionsButton;
            _statsButton.Pressed += StatsButton;
            _exitButton.Pressed += ExitButton;
            Ready += OnReady;
        }

        private void OnReady()
        {
            _menuAnimation.Play("bob");
            _menuAnimation.Play("fade");
        }

        public override void _ExitTree()
        {
            if (_startButton != null) _startButton.Pressed -= StartButton;
            if (_exitButton != null) _exitButton.Pressed -= ExitButton;
            if (_optionsButton != null) _optionsButton.Pressed -= OptionsButton;
        }

        private void StartButton()
        {
            if (_fadeRect == null) return;
            _menuAnimation.PlayBackwards("fade");
            _fadeRect.FadeIn(false);

            GetTree().CreateTimer(0.5f).Timeout += () =>
            {
                if (!IsInstanceValid(this)) return;
                GetTree().ChangeSceneToFile("res://scenes/Game.tscn");
            };
        }

        private void OptionsButton()
        {
            _menuAnimation.PlayBackwards("fade");
            _optionAnimations.Play("options");
        }

        private void StatsButton()
        {
        }

        private void ExitButton()
        {
            if (OS.GetName() == "Web")
            {
                GD.Print("Exit ignored on Web platform.");
                return;
            }
            GetTree().Quit();
        }
    }
}
