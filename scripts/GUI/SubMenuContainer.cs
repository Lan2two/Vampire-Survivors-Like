namespace SurvivorsGame
{
    using Godot;
    [GlobalClass]
    public partial class SubMenuContainer : Control
    {
        [Export] public string animName;
        private Button _backButton;
        private AnimationPlayer _menuAnimation;
        private AnimationPlayer _optionAnimations;
        private CarouselMenuContainer _mainMenuContainer;
        public override void _Ready()
        {
            _backButton = GetNodeOrNull<Button>("BackButton");
            _menuAnimation = GetNodeOrNull<AnimationPlayer>("../../MainMenu");
            _optionAnimations = GetNodeOrNull<AnimationPlayer>("../../Menus");
            _mainMenuContainer = GetNode<CarouselMenuContainer>("../../MainMenuMotion/MainMenuContainer");
            _backButton.Pressed += BackButton;
        }

        public virtual void BackButton()
        {
            GD.Print(animName);
            _mainMenuContainer.ResetMouseHoverSuppression();
            _optionAnimations.PlayBackwards(animName);
            _menuAnimation.Play("fade");
        }

    }
}