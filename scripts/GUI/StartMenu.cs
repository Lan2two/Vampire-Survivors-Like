namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    public partial class StartMenu : Control
    {
        private Control _menuContainer;
        private Button _startButton;
        private Button _exitButton;
        private Array<Vector2> _originalPositions = new();
        private ColorRect _fadeRect;
        private int sign = 1;

        public override void _Ready()
        {
            _fadeRect = GetNodeOrNull<ColorRect>("Fade");
            _menuContainer = GetNodeOrNull<Control>("MenuContainer");
            _startButton = GetNodeOrNull<Button>("MenuContainer/StartButton");
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
            foreach (Control child in _menuContainer.GetChildren())
            {
                _originalPositions.Add(child.Position);
                child.Position += new Vector2(-500, 0);
            }
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
            sign *= -1;
            Vector2 initialPosition = _originalPositions[_menuContainer.GetChildren().IndexOf(node)];
            Vector2 offsetPosition = initialPosition + new Vector2(2, 3) * sign;
            float duration = (float)GD.RandRange(1.5f, 1.55f);
            Tween tween = node.CreateTween();
            tween.TweenProperty(node, "position", offsetPosition, duration)
                .SetTrans(Tween.TransitionType.Linear)
                .SetEase(Tween.EaseType.InOut);
            await ToSignal(GetTree().CreateTimer(duration), "timeout");
            Tween tween1 = node.CreateTween();
            tween1.TweenProperty(node, "position", initialPosition, duration)
                .SetTrans(Tween.TransitionType.Linear)
                .SetEase(Tween.EaseType.InOut);
        }

        private void ModulateMenu()
        {
            Color startingModulate = _menuContainer.Modulate;
            startingModulate.A = 0.0f;
            _menuContainer.Modulate = startingModulate;
            Tween tween = _menuContainer.CreateTween();
            tween.TweenProperty(_menuContainer, "modulate:a", 1.0f, 2.0f)
                .SetTrans(Tween.TransitionType.Quad)
                .SetEase(Tween.EaseType.Out);
            foreach (Control child in _menuContainer.GetChildren())
            {
                Vector2 initialPosition = _originalPositions[_menuContainer.GetChildren().IndexOf(child)];
                Tween childTween = child.CreateTween();
                childTween.TweenProperty(child, "position", initialPosition, 1f)
                    .SetTrans(Tween.TransitionType.Quad);
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

        private void ExitButton()
        {
            GetTree().Quit();
        }
    }
}