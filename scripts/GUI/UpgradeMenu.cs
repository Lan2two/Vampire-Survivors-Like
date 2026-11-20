namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class UpgradeMenu : Control
    {
        [Export] public int Rerolls = 2;
        private UpgradeButtonContainer _upgradeButtonsContainer;
        private Button _rerollButton;
        private Button _skipButton;
        private int _rerollCount;

        public override void _Ready()
        {
            _upgradeButtonsContainer = GetNodeOrNull<UpgradeButtonContainer>("UpgradeButtonContainer");
            _rerollButton = GetNodeOrNull<Button>("UpgradeMenuContainer/RerollButton");
            _skipButton = GetNodeOrNull<Button>("UpgradeMenuContainer/SkipButton");
            if (_upgradeButtonsContainer == null)
            {
                GD.PrintErr("UpgradeButtonContainer not found in UpgradeMenu");
                return;
            }
            if (_rerollButton == null)
            {
                GD.PrintErr("RerollButton not found in UpgradeMenu");
                return;
            }
            if (_skipButton == null)
            {
                GD.PrintErr("RerollButton not found in UpgradeMenu");
                return;
            }
            _rerollButton.Pressed += RerollButton;
            _skipButton.Pressed += SkipButton;
            GameScene.GameOverEvent += Hide;
            VisibilityChanged += SetReroll;
            _rerollCount = Rerolls;
            _rerollButton.Text = $"Reroll\n{_rerollCount}/{Rerolls}";
        }

        public override void _ExitTree()
        {
            if (_rerollButton != null)
            {
                _rerollButton.Pressed -= RerollButton;
            }
            if (_skipButton != null)
            {
                _skipButton.Pressed -= SkipButton;
            }
            GameScene.GameOverEvent -= Hide;
        }
        private void SetReroll()
        {
            _rerollCount = Rerolls;
        }

        private void RerollButton()
        {
            if (_rerollCount <= 0)
            {
                return;
            }
            _upgradeButtonsContainer.RandUpgrade();
            _rerollCount--;
            _rerollButton.Text = $"Reroll\n{_rerollCount}/{Rerolls}";

        }

        private void SkipButton()
        {
            Visible = false;
            GetTree().Paused = false;
        }

    }
}