using Godot;

namespace SurvivorsGame
{
    public partial class StartMenu : Control
    {
        private Control _menuContainer;
        private Button _startButton;
        private Button _optionsButton;
        private Button _exitButton;
        private System.Collections.Generic.List<Vector2> _originalPositions = new();
        private Fade _fadeRect;
        private Vector2 _menuPosition;
        private Tween _menuTween;

        public override void _Ready()
        {
            _fadeRect = GetNodeOrNull<Fade>("Fade");
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

            GetTree().ProcessFrame += OnFirstFrame;
        }

        private void OnFirstFrame()
        {
            GetTree().ProcessFrame -= OnFirstFrame;
            InitializeMenuLayout();
        }

        private void InitializeMenuLayout()
        {
            if (_menuContainer == null || !IsInstanceValid(_menuContainer)) return;

            _menuContainer.ForceUpdateTransform();
            _menuPosition = _menuContainer.GlobalPosition;

            foreach (Node child in _menuContainer.GetChildren())
            {
                if (child is Control controlChild)
                {
                    controlChild.ForceUpdateTransform();
                    _originalPositions.Add(controlChild.Position);
                }
            }

            _menuContainer.GlobalPosition = _menuPosition + new Vector2(-2500, 0);
            _menuContainer.Modulate = new Color(1, 1, 1, 0);

            AnimateMenu(fadeIn: true, durationMultiplier: 1.0f);
        }

        public override void _ExitTree()
        {
            if (_startButton != null) _startButton.Pressed -= StartButton;
            if (_exitButton != null) _exitButton.Pressed -= ExitButton;
            if (_optionsButton != null) _optionsButton.Pressed -= OptionsButton;
        }

        private void AnimateMenu(bool fadeIn, float durationMultiplier = 1.0f)
        {
            if (_menuContainer == null || !IsInstanceValid(_menuContainer)) return;
            if (_menuTween != null && IsInstanceValid(_menuTween))
            {
                _menuTween.Kill();
            }
            Vector2 currentPos = _menuContainer.GlobalPosition;
            float currentAlpha = _menuContainer.Modulate.A;
            _menuTween = GetTree().CreateTween();
            _menuTween.SetParallel(true);
            Vector2 targetPos = fadeIn ? _menuPosition : _menuPosition + new Vector2(-2500, 0);
            float targetAlpha = fadeIn ? 1.0f : 0.0f;

            Tween.TransitionType trans = Tween.TransitionType.Quad;
            Tween.EaseType ease = fadeIn ? Tween.EaseType.Out : Tween.EaseType.In;
            _menuTween.TweenMethod(
                Callable.From<Vector2>(pos => _menuContainer.GlobalPosition = pos),
                currentPos,
                targetPos,
                1.0f * durationMultiplier
            ).SetTrans(trans).SetEase(ease);

            _menuTween.TweenMethod(
                Callable.From<float>(alpha => _menuContainer.Modulate = new Color(1, 1, 1, alpha)),
                currentAlpha,
                targetAlpha,
                2.0f * durationMultiplier
            ).SetTrans(trans).SetEase(ease);
        }

        private void StartButton()
        {
            if (_fadeRect == null) return;

            AnimateMenu(fadeIn: false, durationMultiplier: 0.5f);
            _fadeRect.FadeIn(false);

            GetTree().CreateTimer(0.5f).Timeout += () =>
            {
                if (!IsInstanceValid(this)) return;
                GetTree().ChangeSceneToFile("res://scenes/Game.tscn");
            };
        }

        private void OptionsButton()
        {
            AnimateMenu(false, durationMultiplier: 0.5f);
            GetTree().CreateTimer(0.5f).Timeout += () =>
            {
                if (!IsInstanceValid(this)) return;
                AnimateMenu(true, durationMultiplier: 0.5f);
            };
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
