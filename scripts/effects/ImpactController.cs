namespace SurvivorsGame
{
    using Godot;

    public partial class ImpactController : ColorRect
    {
        private ShaderMaterial _shaderMaterial;
        private AnimationPlayer _animationPlayer;

        public override void _Ready()
        {
            _animationPlayer = GetNodeOrNull<AnimationPlayer>("../AnimationPlayer");
            if (Material is ShaderMaterial originalMaterial)
            {
                _shaderMaterial = (ShaderMaterial)originalMaterial.Duplicate();
                Material = _shaderMaterial;
            }

            GetTree().CreateTimer(1, false).Timeout += () => { Impact(new Vector2(0.5f, -0.5f)); };
        }

        public void Impact(Vector2 sliceDirection)
        {
            if (_shaderMaterial == null || _animationPlayer == null)
            {
                return;
            }
            _shaderMaterial.SetShaderParameter("split_normal", sliceDirection);
            _animationPlayer.Stop();
            _animationPlayer.Play("impact");
        }
    }
}
