namespace SurvivorsGame
{
    using Godot;
    public partial class SubMenuContainer : Control
    {
        [Export] public string animName;
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
            GD.Print(animName);
            _optionAnimations.PlayBackwards(animName);
            _menuAnimation.Play("fade");
        }

    }
}