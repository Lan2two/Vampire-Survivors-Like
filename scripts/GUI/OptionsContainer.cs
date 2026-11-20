namespace SurvivorsGame
{
    using Godot;

    public partial class OptionsContainer : SubMenuContainer
    {
        private Button _defaultButton;
        private Button _saveButton;
        private VBoxContainer _rebindContainer;

        private bool _saved = true;
        private bool _confirmDiscard = false;

        public OptionsContainer()
        {
            animName = "options";
        }

        public override void _Ready()
        {
            base._Ready();
            _defaultButton = GetNodeOrNull<Button>("%DefaultButton");
            _saveButton = GetNodeOrNull<Button>("%SaveButton");
            _rebindContainer = GetNodeOrNull<VBoxContainer>("%RebindContainer");

            if (_rebindContainer == null)
            {
                GD.PrintErr("RebindContainer not found in Options Container");
            }

            AddToGroup("OptionsMenu");

            _defaultButton.Pressed += DefaultButton;
            _saveButton.Pressed += SaveButton;
        }

        // Call this from RebindButton whenever a keybind is modified
        public void MarkUnsaved()
        {
            _saved = false;
            ResetBackButtonState();
        }

        public override void BackButton()
        {
            if (!_saved)
            {
                if (!_confirmDiscard)
                {
                    _confirmDiscard = true;
                    if (_backButton != null)
                        _backButton.Text = "Discard & Back?";
                    return;
                }

                // Second click: revert InputMap to last saved file
                SaveSystem.LoadAndApplyKeybinds();
                RefreshAllButtons();
                _saved = true;
            }

            ResetBackButtonState();
            base.BackButton();
        }

        private void DefaultButton()
        {
            SaveSystem.ResetKeybinds();
            RefreshAllButtons();
            _saved = false;
            ResetBackButtonState();
        }

        private void SaveButton()
        {
            SaveSystem.SaveKeybinds();
            _saved = true;
            ResetBackButtonState();
        }

        private void ResetBackButtonState()
        {
            _confirmDiscard = false;
            if (_backButton != null)
                _backButton.Text = "Back";
        }

        private void RefreshAllButtons()
        {
            foreach (Node child in _rebindContainer.GetChildren())
            {
                if (child is RebindButton button)
                {
                    button.UpdateButtonText();
                }
            }
        }

        private bool IsConnectedToBackButton()
        {
            // Prevents double-firing if base SubMenuContainer already wired up _backButton.Pressed
            return _backButton.IsConnected(Button.SignalName.Pressed, new Callable(this, nameof(BackButton)));
        }
    }
}
