namespace SurvivorsGame
{
    using Godot;
    using System;
    [GlobalClass]
    public partial class CustomButton : Button
    {
        public event Action<CustomButton> CustomButtonPressed;
        public event Action<CustomButton> CustomButtonHovered;

        public override void _Ready()
        {
            Pressed += OnButtonPressed;
            MouseEntered += OnMouseEntered;
        }

        private void OnButtonPressed()
        {
            CustomButtonPressed?.Invoke(this);
        }

        private void OnMouseEntered()
        {
            CustomButtonHovered?.Invoke(this);
        }
    }
}