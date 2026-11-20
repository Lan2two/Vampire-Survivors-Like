namespace SurvivorsGame
{
    using Godot;

    [Tool]
    public partial class MainMenuContainer : Node2D
    {
        [Export] public float Spacing { get; set; } = 50.0f;
        [Export] public bool WraparoundEnabled { get; set; } = false;
        [Export] public float Radius { get; set; } = 300.0f;
        [Export] public float WraparoundWidth { get; set; } = 100.0f;

        [Export(PropertyHint.Range, "0.1,5.0,0.1")] public float OpacityStrength { get; set; } = 1.0f;
        [Export(PropertyHint.Range, "0.1,5.0,0.1")] public float ScaleStrength { get; set; } = 1.0f;
        [Export(PropertyHint.Range, "0.0,1.0,0.05")] public float ScaleMin { get; set; } = 0.5f;
        [Export] public float SmoothingSpeed { get; set; } = 10.0f;

        [Export]
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (PositionOffsetNode != null && PositionOffsetNode.GetChildCount() > 0)
                {
                    _selectedIndex = Mathf.Clamp(value, 0, PositionOffsetNode.GetChildCount() - 1);
                }
                else
                {
                    _selectedIndex = Mathf.Max(0, value);
                }
            }
        }
        private int _selectedIndex = 0;

        [Export] public bool FollowButtonFocus { get; set; } = true;
        [Export] public bool FollowMouseHover { get; set; } = true;
        [Export] public bool UseMouseWheel { get; set; } = true;
        [Export] public Control PositionOffsetNode { get; set; }

        private bool _ignoreMouseHover = false;
        private Vector2 _lastMousePos = Vector2.Zero;

        public override void _Ready()
        {
            if (!Engine.IsEditorHint())
            {
                _lastMousePos = GetViewport().GetMousePosition();
            }
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (PositionOffsetNode == null || PositionOffsetNode.GetChildCount() == 0) return;

            if (@event is InputEventMouseMotion motion)
            {
                if (motion.Position.DistanceTo(_lastMousePos) > 5.0f)
                {
                    _ignoreMouseHover = false;
                    _lastMousePos = motion.Position;
                }
            }

            if (UseMouseWheel && @event is InputEventMouseButton mouseButton && mouseButton.Pressed)
            {
                if (mouseButton.ButtonIndex == MouseButton.WheelUp)
                {
                    ScrollTo(SelectedIndex - 1);
                    GetViewport().SetInputAsHandled();
                }
                else if (mouseButton.ButtonIndex == MouseButton.WheelDown)
                {
                    ScrollTo(SelectedIndex + 1);
                    GetViewport().SetInputAsHandled();
                }
            }
        }

        private void ScrollTo(int newIndex)
        {
            _ignoreMouseHover = true;
            SelectedIndex = newIndex;
            UpdateFocusAndStyles();
        }

        private void UpdateFocusAndStyles()
        {
            var children = PositionOffsetNode.GetChildren();
            if (SelectedIndex < 0 || SelectedIndex >= children.Count) return;

            for (int i = 0; i < children.Count; i++)
            {
                if (children[i] is Control ctrl)
                {
                    if (i == SelectedIndex)
                    {
                        ctrl.FocusMode = Control.FocusModeEnum.All;
                        ctrl.GrabFocus();
                        ctrl.QueueRedraw();
                    }
                    else
                    {
                        if (ctrl.HasFocus())
                        {
                            ctrl.ReleaseFocus();
                        }
                    }
                }
            }
        }

        public override void _Process(double delta)
        {
            if (PositionOffsetNode == null) return;
            var children = PositionOffsetNode.GetChildren();
            if (children.Count == 0) return;

            SelectedIndex = Mathf.Clamp(SelectedIndex, 0, children.Count - 1);
            float dt = (float)delta;

            if (!Engine.IsEditorHint())
            {
                if (FollowButtonFocus)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        if (children[i] is Control control && control.HasFocus())
                        {
                            if (SelectedIndex != i)
                            {
                                SelectedIndex = i;
                                _ignoreMouseHover = true;
                            }
                            break;
                        }
                    }
                }

                if (FollowMouseHover && !_ignoreMouseHover)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        if (children[i] is Control control)
                        {
                            if (control.GetGlobalRect().HasPoint(control.GetGlobalMousePosition()))
                            {
                                if (SelectedIndex != i)
                                {
                                    SelectedIndex = i;
                                    UpdateFocusAndStyles();
                                }
                                break;
                            }
                        }
                    }
                }
            }

            if (WraparoundEnabled)
            {
                float maxIndexRange = Mathf.Max(1.0f, children.Count / 2.0f);

                for (int i = 0; i < children.Count; i++)
                {
                    if (children[i] is not Control child) continue;

                    float indexDiff = (i - SelectedIndex) / maxIndexRange;
                    float clampedDiff = Mathf.Clamp(indexDiff, -1.0f, 1.0f);
                    float angle = clampedDiff * Mathf.Pi;

                    float posX = -Mathf.Cos(angle) * WraparoundWidth;
                    float posY = Mathf.Sin(angle) * Radius;

                    Vector2 targetPos = new Vector2(posX, posY) - (child.Size / 2.0f);
                    child.Position = child.Position.Lerp(targetPos, SmoothingSpeed * dt);

                    ApplyVisuals(child, i, dt);
                }

                PositionOffsetNode.Position = PositionOffsetNode.Position.Lerp(Vector2.Zero, SmoothingSpeed * dt);
            }
            else
            {
                float currentY = 0.0f;

                for (int i = 0; i < children.Count; i++)
                {
                    if (children[i] is not Control child) continue;

                    float targetPosY = currentY;
                    currentY += child.Size.Y + Spacing;

                    Vector2 targetPos = new Vector2(-child.Size.X / 2.0f, targetPosY);
                    child.Position = targetPos;

                    ApplyVisuals(child, i, dt);
                }

                if (children[SelectedIndex] is Control selectedChild)
                {
                    float finalTargetOffsetY = -(selectedChild.Position.Y + (selectedChild.Size.Y / 2.0f));
                    Vector2 targetOffsetPos = new Vector2(0.0f, finalTargetOffsetY);
                    PositionOffsetNode.Position = PositionOffsetNode.Position.Lerp(targetOffsetPos, SmoothingSpeed * dt);
                }
            }

            if (Engine.IsEditorHint())
            {
                QueueRedraw();
            }
        }

        private void ApplyVisuals(Control child, int index, float dt)
        {
            float distance = Mathf.Abs(index - SelectedIndex);

            // Scale effect
            float targetScaleVal = Mathf.Clamp(1.0f - (distance * 0.2f * ScaleStrength), ScaleMin, 1.0f);
            Vector2 targetScale = new Vector2(targetScaleVal, targetScaleVal);
            child.Scale = child.Scale.Lerp(targetScale, SmoothingSpeed * dt);

            // Opacity / Modulate alpha
            float targetAlpha = Mathf.Clamp(1.0f - (distance * 0.3f * OpacityStrength), 0.0f, 1.0f);
            Color modulate = child.Modulate;
            modulate.A = Mathf.Lerp(modulate.A, targetAlpha, SmoothingSpeed * dt);
            child.Modulate = modulate;

            // Z-Index and Focus/Mouse Mode management
            if (index == SelectedIndex)
            {
                child.ZIndex = 1;
                child.MouseFilter = Control.MouseFilterEnum.Stop;
                if (child.FocusMode == Control.FocusModeEnum.None)
                {
                    child.FocusMode = Control.FocusModeEnum.All;
                }
            }
            else
            {
                child.ZIndex = -Mathf.RoundToInt(distance);
                child.MouseFilter = Control.MouseFilterEnum.Ignore;
                child.FocusMode = Control.FocusModeEnum.None;
            }
        }

        public void MoveUp()
        {
            if (PositionOffsetNode == null) return;
            ScrollTo(SelectedIndex - 1);
        }

        public void MoveDown()
        {
            if (PositionOffsetNode == null) return;
            ScrollTo(SelectedIndex + 1);
        }
    }
}