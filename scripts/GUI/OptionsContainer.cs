namespace SurvivorsGame
{
    using Godot;
    public partial class OptionsContainer : Control
    {
        private Button _backButton;
        private AnimationPlayer _menuAnimation;
        private AnimationPlayer _optionAnimations;
        public override void _Ready()
        {
            _backButton = GetNodeOrNull<Button>("BackButton");
            _menuAnimation = GetNodeOrNull<AnimationPlayer>("../../MainMenu");
            _optionAnimations = GetNodeOrNull<AnimationPlayer>("../../Menus");
            _backButton.Pressed += BackButton;
        }

        private void BackButton()
        {
            _optionAnimations.PlayBackwards("options");
            _menuAnimation.Play("fade");
        }

    }
}