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
        private ColorRect _fadeRect;
        private Vector2 _menuPosition;
        private bool _isAnimating = false;
        private Tween _menuTween;

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
            _menuContainer.GlobalPosition = _menuPosition + new Vector2(-2000, 0);
            _menuContainer.Modulate = new Color(1, 1, 1, 0);

            AnimateMenu(fadeIn: true, durationMultiplier: 1.0f);
        }

        public override void _ExitTree()
        {
            GetTree().ProcessFrame -= OnFirstFrame;
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

            _menuTween = GetTree().CreateTween();
            _menuTween.SetParallel(true);
            SetButtonsInteractivity(false);

            Vector2 startPos = _menuContainer.GlobalPosition;
            Vector2 targetPos = fadeIn ? _menuPosition : _menuPosition + new Vector2(-2000, 0);

            float startAlpha = _menuContainer.Modulate.A;
            float targetAlpha = fadeIn ? 1.0f : 0.0f;

            Tween.TransitionType trans = fadeIn ? Tween.TransitionType.Quad : Tween.TransitionType.Quad;
            Tween.EaseType ease = fadeIn ? Tween.EaseType.Out : Tween.EaseType.In;

            _menuTween.TweenMethod(Callable.From<Vector2>(pos => _menuContainer.GlobalPosition = pos), startPos, targetPos, 1.0f * durationMultiplier)
                .SetTrans(trans)
                .SetEase(ease);

            _menuTween.TweenMethod(Callable.From<float>(alpha => _menuContainer.Modulate = new Color(1, 1, 1, alpha)), startAlpha, targetAlpha, 2.0f * durationMultiplier)
                .SetTrans(trans)
                .SetEase(ease);

            _menuTween.Chain().TweenCallback(Callable.From(() =>
            {
                SetButtonsInteractivity(true);
                _isAnimating = false;
            }));
        }

        private void SetButtonsInteractivity(bool enable)
        {
            MouseFilterEnum filter = enable ? MouseFilterEnum.Stop : MouseFilterEnum.Ignore;
            if (_startButton != null) _startButton.MouseFilter = filter;
            if (_optionsButton != null) _optionsButton.MouseFilter = filter;
            if (_exitButton != null) _exitButton.MouseFilter = filter;
        }

        private void StartButton()
        {
            if (_isAnimating || _fadeRect == null) return;
            _isAnimating = true;

            SetButtonsInteractivity(false);
            float startAlpha = _fadeRect.Modulate.A;

            Tween fadeTween = GetTree().CreateTween();
            fadeTween.TweenMethod(Callable.From<float>(alpha => _fadeRect.Modulate = new Color(_fadeRect.Modulate.R, _fadeRect.Modulate.G, _fadeRect.Modulate.B, alpha)), startAlpha, 1.0f, 0.5f);

            fadeTween.Chain().TweenCallback(Callable.From(() =>
            {
                GetTree().ChangeSceneToFile("res://scenes/Game.tscn");
            }));
        }

        private void OptionsButton()
        {
            if (_isAnimating) return;
            _isAnimating = true;

            if (_menuTween != null && IsInstanceValid(_menuTween))
            {
                _menuTween.Kill();
            }

            _menuTween = GetTree().CreateTween();
            _menuTween.SetParallel(true);
            SetButtonsInteractivity(false);

            Vector2 outTargetPos = _menuPosition + new Vector2(-1000, 0);
            _menuTween.TweenMethod(Callable.From<Vector2>(pos => _menuContainer.GlobalPosition = pos), _menuPosition, outTargetPos, 1.0f)
                .SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.In);
            _menuTween.TweenMethod(Callable.From<float>(alpha => _menuContainer.Modulate = new Color(1, 1, 1, alpha)), 1.0f, 0.0f, 1.5f)
                .SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.In);
            Tween pipelineChain = _menuTween.Chain();
            pipelineChain.TweenInterval(1.5f);
            pipelineChain.TweenCallback(Callable.From(() =>
            {
                if (!IsInstanceValid(this)) return;

                Tween returnTween = GetTree().CreateTween();
                returnTween.SetParallel(true);

                returnTween.TweenMethod(Callable.From<Vector2>(pos => _menuContainer.GlobalPosition = pos), outTargetPos, _menuPosition, 1.0f)
                    .SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
                returnTween.TweenMethod(Callable.From<float>(alpha => _menuContainer.Modulate = new Color(1, 1, 1, alpha)), 0.0f, 1.0f, 1.5f)
                    .SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);

                returnTween.Chain().TweenCallback(Callable.From(() =>
                {
                    SetButtonsInteractivity(true);
                    _isAnimating = false;
                }));
            }));
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
