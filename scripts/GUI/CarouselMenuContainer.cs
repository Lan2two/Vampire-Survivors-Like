namespace SurvivorsGame
{
    using Godot;

#if !GODOT_WEB
    [Tool]
#endif
    public partial class CarouselMenuContainer : Node2D
    {
        // Tracks the currently active carousel across the scene
        public static CarouselMenuContainer CurrentFocused { get; private set; }

        [Export] public bool IsHorizontal { get; set; } = false;
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

        // New Focus Controls
        [Export] public bool StartFocused { get; set; } = false;
        [Export] public bool FocusOnHover { get; set; } = true;

        [Export] public Control PositionOffsetNode { get; set; }

        public bool IsFocused => ReferenceEquals(CurrentFocused, this);

        private bool _ignoreMouseHover = false;
        private Vector2 _lastMousePos = Vector2.Zero;

        public override void _Ready()
        {
            if (!Engine.IsEditorHint())
            {
                _lastMousePos = GetViewport().GetMousePosition();
                if (StartFocused || !GodotObject.IsInstanceValid(CurrentFocused))
                {
                    SetFocused();
                }
            }
        }

        public override void _ExitTree()
        {
            if (ReferenceEquals(CurrentFocused, this))
            {
                CurrentFocused = null;
            }
        }

        /// <summary>
        /// Call this from your UI controller (or via button/tab signals) to focus this specific carousel.
        /// </summary>
        public void SetFocused()
        {
            if (ReferenceEquals(CurrentFocused, this)) return;

            CarouselMenuContainer previous = CurrentFocused;
            CurrentFocused = this;

            ResetMouseHoverSuppression();

            if (GodotObject.IsInstanceValid(previous))
            {
                previous.ClearChildFocus();
            }

            UpdateFocusAndStyles();
        }

        public void ClearChildFocus()
        {
            if (PositionOffsetNode == null) return;
            foreach (Node child in PositionOffsetNode.GetChildren())
            {
                if (child is Control ctrl && ctrl.HasFocus())
                {
                    ctrl.ReleaseFocus();
                }
            }
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (PositionOffsetNode == null || PositionOffsetNode.GetChildCount() == 0) return;

            if (!IsFocused) return;

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
            if (!IsFocused) SetFocused();
            _ignoreMouseHover = true;
            SelectedIndex = newIndex;
            UpdateFocusAndStyles();
        }

        public void ResetMouseHoverSuppression()
        {
            _ignoreMouseHover = false;
            _lastMousePos = GetViewport().GetMousePosition();
        }

        private void UpdateFocusAndStyles()
        {
            if (!IsFocused || PositionOffsetNode == null) return;

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
                    else if (ctrl.HasFocus())
                    {
                        ctrl.ReleaseFocus();
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
                // Reliable mouse movement check that works even when UI controls consume input events
                Vector2 currentMousePos = GetViewport().GetMousePosition();
                if (currentMousePos.DistanceTo(_lastMousePos) > 5.0f)
                {
                    _ignoreMouseHover = false;
                    _lastMousePos = currentMousePos;
                }

                // 1. Check if a child inside this carousel gained focus (e.g., via keyboard/gamepad)
                if (FollowButtonFocus)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        if (children[i] is Control control && control.HasFocus())
                        {
                            if (!IsFocused) SetFocused();
                            if (SelectedIndex != i)
                            {
                                SelectedIndex = i;
                                _ignoreMouseHover = true;
                            }
                            break;
                        }
                    }
                }

                // 2. Check mouse hover
                if (FollowMouseHover && !_ignoreMouseHover)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        if (children[i] is Control control && control.GetGlobalRect().HasPoint(control.GetGlobalMousePosition()))
                        {
                            if (!IsFocused && FocusOnHover)
                            {
                                SetFocused();
                            }

                            if (IsFocused && SelectedIndex != i)
                            {
                                SelectedIndex = i;
                                UpdateFocusAndStyles();
                            }
                            break;
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
                    Vector2 targetPos;
                    if (IsHorizontal)
                    {
                        float posX = Mathf.Sin(angle) * Radius;
                        float posY = -Mathf.Cos(angle) * WraparoundWidth;
                        targetPos = new Vector2(posX, posY) - (child.Size / 2.0f);
                    }
                    else
                    {
                        float posX = -Mathf.Cos(angle) * WraparoundWidth;
                        float posY = Mathf.Sin(angle) * Radius;
                        targetPos = new Vector2(posX, posY) - (child.Size / 2.0f);
                    }
                    child.Position = child.Position.Lerp(targetPos, SmoothingSpeed * dt);
                    ApplyVisuals(child, i, dt);
                }
                PositionOffsetNode.Position = PositionOffsetNode.Position.Lerp(Vector2.Zero, SmoothingSpeed * dt);
            }
            else
            {
                float currentOffset = 0.0f;
                for (int i = 0; i < children.Count; i++)
                {
                    if (children[i] is not Control child) continue;
                    Vector2 targetPos;
                    if (IsHorizontal)
                    {
                        targetPos = new Vector2(currentOffset, -child.Size.Y / 2.0f);
                        currentOffset += child.Size.X + Spacing;
                    }
                    else
                    {
                        targetPos = new Vector2(-child.Size.X / 2.0f, currentOffset);
                        currentOffset += child.Size.Y + Spacing;
                    }
                    child.Position = targetPos;
                    ApplyVisuals(child, i, dt);
                }
                if (children[SelectedIndex] is Control selectedChild)
                {
                    Vector2 targetOffsetPos;
                    if (IsHorizontal)
                    {
                        float finalTargetOffsetX = -(selectedChild.Position.X + (selectedChild.Size.X / 2.0f));
                        targetOffsetPos = new Vector2(finalTargetOffsetX, 0.0f);
                    }
                    else
                    {
                        float finalTargetOffsetY = -(selectedChild.Position.Y + (selectedChild.Size.Y / 2.0f));
                        targetOffsetPos = new Vector2(0.0f, finalTargetOffsetY);
                    }
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
        public void MoveLeft() => MoveUp();
        public void MoveRight() => MoveDown();
    }
}
