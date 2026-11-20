namespace SurvivorsGame
{
    using Godot;
    using System;
    public partial class UpgradeButton : Button
    {
        public event Action<UpgradeButton> UpgradeButtonPressed;

        public override void _Ready()
        {
            Pressed += OnButtonPressed;
        }

        private void OnButtonPressed()
        {
            UpgradeButtonPressed?.Invoke(this);
        }
    }
}