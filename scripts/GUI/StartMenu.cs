namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    public partial class StartMenu : Control
    {
        private Control _menuContainer;
        private Button _startButton;
        private Button _optionsButton;
        private Button _exitButton;
        private Array<Vector2> _originalPositions = new();
        private Array<Color> _originalColors = new();
        private ColorRect _fadeRect;
        private int sign = 1;
        private Color _menuColor;
        private Vector2 _menuPosition;
        private bool _isAnimating = false;

        public override void _Ready()
        {
            _fadeRect = GetNodeOrNull<ColorRect>("Fade");
            _menuContainer = GetNodeOrNull<Control>("MenuContainer");
            _startButton = GetNodeOrNull<Button>("MenuContainer/StartButton");
            _optionsButton = GetNodeOrNull<Button>("MenuContainer/OptionsButton");
            _exitButton = GetNodeOrNull<Button>("MenuContainer/ExitButton");
            if (_menuContainer == null)
            {
                GD.PrintErr("MenuContainer not found in StartMenu");
                return;
            }
            if (_startButton == null)
            {
                GD.PrintErr("StartButton not found in StartMenu");
                return;
            }
            if (_exitButton == null)
            {
                GD.PrintErr("ExitButton not found in StartMenu");
                return;
            }
            _startButton.Pressed += StartButton;
            _exitButton.Pressed += ExitButton;
            _optionsButton.Pressed += OptionsButton;
            _menuColor = _menuContainer.Modulate;
            _menuPosition = _menuContainer.Position;
            foreach (Control child in _menuContainer.GetChildren())
            {
                _originalPositions.Add(child.Position);
                _originalColors.Add(child.Modulate);
            }
            _menuContainer.Position += new Vector2(-1000, 0);
            ModulateMenu();
        }

        public override void _ExitTree()
        {
            if (_startButton != null)
            {
                _startButton.Pressed -= StartButton;
            }
            if (_exitButton != null)
            {
                _exitButton.Pressed -= ExitButton;
            }
        }

        public override void _Process(double delta)
        {
            for (int i = 0; i < _menuContainer.GetChildren().Count; i++)
            {
                Control child = _menuContainer.GetChildren()[i] as Control;
                if (child == null)
                {
                    return;
                }
                if (child.Position == _originalPositions[i])
                {
                    AnimateMenu(child);
                }
            }
        }

        private async void AnimateMenu(Control node)
        {
            if (_isAnimating)
            {
                return;
            }
            sign *= -1;
            Vector2 initialPosition = _originalPositions[_menuContainer.GetChildren().IndexOf(node)];
            Vector2 offsetPosition = initialPosition + new Vector2(2, 3) * sign;
            float duration = 1.5f;
            Tween tween = node.CreateTween();
            tween.SetParallel(true);
            tween.TweenProperty(node, "position", offsetPosition, duration)
                .SetTrans(Tween.TransitionType.Linear)
                .SetEase(Tween.EaseType.InOut);
            tween.Finished += tween.Dispose;
            await ToSignal(GetTree().CreateTimer(duration), "timeout");
            Tween tween1 = node.CreateTween();
            tween1.SetParallel(true);
            tween1.TweenProperty(node, "position", initialPosition, duration)
                .SetTrans(Tween.TransitionType.Linear)
                .SetEase(Tween.EaseType.InOut);
            tween1.Finished += tween.Dispose;
        }

        private void ModulateMenu(bool fadeIn = true)
        {
            Color startingModulate = _menuContainer.Modulate;
            if (fadeIn)
            {
                startingModulate.A = 0.0f;
                _menuContainer.Modulate = startingModulate;
                Tween tween = _menuContainer.CreateTween();
                tween.SetParallel(true);
                tween.TweenProperty(_menuContainer, "modulate:a", 1.0f, 2.0f)
                    .SetTrans(Tween.TransitionType.Quad)
                    .SetEase(Tween.EaseType.Out);
                tween.TweenProperty(_menuContainer, "position", _menuPosition, 1.0f)
                    .SetTrans(Tween.TransitionType.Quad)
                    .SetEase(Tween.EaseType.Out);
                tween.Finished += tween.Dispose;
            }
            else
            {
                Tween tween = _menuContainer.CreateTween();
                tween.SetParallel(true);
                tween.TweenProperty(_menuContainer, "modulate:a", 0.0f, 2.0f)
                    .SetTrans(Tween.TransitionType.Quad)
                    .SetEase(Tween.EaseType.In);
                tween.TweenProperty(_menuContainer, "position", _menuPosition + new Vector2(-1000, 0), 1.0f)
                    .SetTrans(Tween.TransitionType.Quad)
                    .SetEase(Tween.EaseType.In);
                tween.Finished += tween.Dispose;
            }
        }

        private async void StartButton()
        {
            if (_fadeRect != null)
            {
                Tween fadeTween = _fadeRect.CreateTween();
                fadeTween.TweenProperty(_fadeRect, "color:a", 1.0f, .5f)
                    .SetTrans(Tween.TransitionType.Linear);
                await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
            }
            GetTree().ChangeSceneToFile("res://scenes/Game.tscn");
        }

        private async void OptionsButton()
        {
            if (!_isAnimating)
            {
                _isAnimating = true;
                ModulateMenu(false);
                await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
                ModulateMenu(true);
                _isAnimating = false;
            }
        }

        private void ExitButton()
        {
            GetTree().Quit();
        }
    }
}