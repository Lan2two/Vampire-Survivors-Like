namespace SurvivorsGame
{
    using Godot;

    public partial class RebindButton : Button
    {
        [Export] public string ActionName { get; set; } = "ui_accept";
        private Label _label;
        private bool _listenForInput = false;

        public override void _Ready()
        {
            _label = GetNodeOrNull<Label>("Label");
            if (_label == null)
            {
                GD.PrintErr("Label not found in RebindKey");
                return;
            }
            _label.Text = ActionName.Capitalize();
            FocusMode = FocusModeEnum.None;
            Pressed += OnButtonPressed;
            UpdateButtonText();
        }

        private void OnButtonPressed()
        {
            _listenForInput = true;
            Text = "Press any key...";
        }

        public override void _Input(InputEvent @event)
        {
            if (!_listenForInput) return;

            bool ValidInput = @event is InputEventKey keyevent && keyevent.Pressed && !keyevent.Echo ||
            @event is InputEventMouseButton mouseEvent && mouseEvent.Pressed;

            if (ValidInput)
            {
                _listenForInput = false;
                if (@event is InputEventKey pressedKey && pressedKey.Pressed && !pressedKey.Echo && pressedKey.Keycode == Key.Escape)
                {
                    InputMap.ActionEraseEvents(ActionName);
                }
                else
                {
                    RebindAction(@event);
                }
                GetTree().CallGroup("OptionsMenu", nameof(OptionsContainer.MarkUnsaved));
                UpdateButtonText();

                GetViewport().SetInputAsHandled();
            }
        }

        private void RebindAction(InputEvent newEvent)
        {
            InputMap.ActionEraseEvents(ActionName);
            InputMap.ActionAddEvent(ActionName, newEvent);
        }

        public void UpdateButtonText()
        {
            var events = InputMap.ActionGetEvents(ActionName);
            if (events.Count > 0)
            {
                Text = events[0].AsText().Replace(" - Physical", "").Capitalize();
                //GD.Print(events[0].AsText());
            }
            else
            {
                Text = "Unassigned";
            }
        }
    }
}