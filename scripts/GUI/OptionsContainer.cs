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
            Node _parent = GetParent();
            _backButton = GetNodeOrNull<Button>("BackButton");
            _menuAnimation = _parent.GetNodeOrNull<AnimationPlayer>("AnimationPlayers/MainMenu");
            _optionAnimations = _parent.GetNodeOrNull<AnimationPlayer>("AnimationPlayers/Options");
            _backButton.Pressed += BackButton;
        }

        private void BackButton()
        {
            _optionAnimations.PlayBackwards("options");
            _menuAnimation.Play("fade");
        }

    }
}