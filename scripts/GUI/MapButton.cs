namespace SurvivorsGame
{
    using Godot;

    public partial class MapButton : TextureButton
    {
        [Export] public PackedScene packedScene;
        private AnimationPlayer _optionAnimations;

        public override void _Ready()
        {
            _optionAnimations = GetNodeOrNull<AnimationPlayer>("%Menus");
            SetProcess(false);
            Pressed += OnPressed;
        }

        private void OnPressed()
        {
            _optionAnimations.PlayBackwards("game");
            SceneLoader.Instance.LoadScene(packedScene.ResourcePath);
        }
    }
}