namespace SurvivorsGame
{
    using Godot;

    public partial class StartMenu : Control
    {
        private Control _menuContainer;
        private Button _startButton;
        private Button _optionsButton;
        private Button _exitButton;
        private System.Collections.Generic.List<Vector2> _originalPositions = new();
        private ColorRect _fadeRect;
        private int sign = 1;
        private Vector2 _menuPosition;
        private bool _isAnimating = false;
        private Tween _menuModulateTween;

        public override void _Ready()
        {
            _fadeRect = GetNodeOrNull<ColorRect>("Fade");
            _menuContainer = GetNodeOrNull<Control>("MenuContainer");
            _startButton = GetNodeOrNull<Button>("MenuContainer/StartButton");
            _optionsButton = GetNodeOrNull<Button>("MenuContainer/OptionsButton");
            _exitButton = GetNodeOrNull<Button>("MenuContainer/ExitButton");

            if (_menuContainer == null || _startButton == null || _exitButton == null)
            {
                GD.PrintErr("Required UI components missing in StartMenu");
                return;
            }

            _startButton.Pressed += StartButton;
            _exitButton.Pressed += ExitButton;
            _optionsButton.Pressed += OptionsButton;

            InitializeMenuLayout();
        }

        private void InitializeMenuLayout()
        {
            if (_menuContainer == null || !IsInstanceValid(_menuContainer)) return;
            _menuContainer.ForceUpdateTransform();
            _menuPosition = _menuContainer.Position;
            foreach (Node child in _menuContainer.GetChildren())
            {
                if (child is Control controlChild)
                {
                    controlChild.ForceUpdateTransform();
                    _originalPositions.Add(controlChild.Position);
                }
            }
            _menuContainer.Position = _menuPosition + new Vector2(-1000, 0);
            _menuContainer.Modulate = new Color(1, 1, 1, 0);
            ModulateMenu(true);
        }

        public override void _ExitTree()
        {
            if (_startButton != null) _startButton.Pressed -= StartButton;
            if (_exitButton != null) _exitButton.Pressed -= ExitButton;
            if (_optionsButton != null) _optionsButton.Pressed -= OptionsButton;
        }

        private void ModulateMenu(bool fadeIn = true)
        {
            if (_menuContainer == null || !IsInstanceValid(_menuContainer)) return;

            if (_menuModulateTween != null && IsInstanceValid(_menuModulateTween))
            {
                _menuModulateTween.Kill();
            }

            _menuModulateTween = _menuContainer.CreateTween();
            _menuModulateTween.SetParallel(true);

            if (fadeIn)
            {
                _menuModulateTween.TweenProperty(_menuContainer, "modulate", new Color(1, 1, 1, 1), 2.0f).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
                _menuModulateTween.TweenProperty(_menuContainer, "position", _menuPosition, 1.0f).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
            }
            else
            {
                _menuModulateTween.TweenProperty(_menuContainer, "modulate", new Color(1, 1, 1, 0), 2.0f).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.In);
                _menuModulateTween.TweenProperty(_menuContainer, "position", _menuPosition + new Vector2(-1000, 0), 1.0f).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.In);
            }
        }

        private void StartButton()
        {
            if (_fadeRect != null)
            {
                Tween fadeTween = _fadeRect.CreateTween();
                fadeTween.TweenProperty(_fadeRect, "color", new Color(_fadeRect.Color.R, _fadeRect.Color.G, _fadeRect.Color.B, 1.0f), 0.5f).SetTrans(Tween.TransitionType.Linear);
                fadeTween.TweenCallback(Callable.From(() =>
                {
                    GetTree().ChangeSceneToFile("res://scenes/Game.tscn");
                })).SetDelay(1.0f);
            }
            else
            {
                GetTree().ChangeSceneToFile("res://scenes/Game.tscn");
            }
        }

        private void OptionsButton()
        {
            if (!_isAnimating)
            {
                _isAnimating = true;
                ModulateMenu(false);
                GetTree().CreateTimer(2.0f).Timeout += () =>
                {
                    if (!IsInstanceValid(this)) return;
                    ModulateMenu(true);
                    _isAnimating = false;
                };
            }
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
