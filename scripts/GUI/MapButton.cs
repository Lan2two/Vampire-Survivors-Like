namespace SurvivorsGame
{
    using Godot;

    public partial class MapButton : TextureButton
    {
        [Export] public PackedScene packedScene;
        private Fade _fadeRect;
        private AnimationPlayer _optionAnimations;

        public override void _Ready()
        {
            _fadeRect = GetNodeOrNull<Fade>("%Fade");
            _optionAnimations = GetNodeOrNull<AnimationPlayer>("%Menus");
            Pressed += OnPressed;
        }

        private void OnPressed()
        {
            if (_fadeRect == null) return;
            if (packedScene == null)
            {
                GD.PrintErr("Scene is null");
                return;
            }
            _optionAnimations.PlayBackwards("game");
            _fadeRect.FadeIn(false);

            GetTree().CreateTimer(_fadeRect.fadeDuration).Timeout += () =>
            {
                if (!IsInstanceValid(this)) return;
                GetTree().ChangeSceneToFile(packedScene.ResourcePath);
            };
        }
    }
}