namespace SurvivorsGame
{
    using Godot;

    public partial class StatusDisplay : Node2D
    {
        public TextureProgressBar ProgressBar;
        private AnimatedSprite2D _animatedSprite;
        private Vector2 _progressbarOffset;
        private StatusEffect effect;

        public override void _Ready()
        {
            ProgressBar = GetNodeOrNull<TextureProgressBar>("Display");
            if (ProgressBar == null)
            {
                GD.PrintErr("StatusDisplay couldn't find Display");
                return;
            }

            GlobalScale = Vector2.One;
            ProgressBar.Step = 0.01;
        }

        public void SetUp(StatusEffect status, Node node)
        {
            effect = status;
            ProgressBar.MaxValue = effect.Duration;
            _animatedSprite = node.GetComponent<AnimatedSprite2D>();
            UpdateProgressBarScale();
        }
        public override void _Process(double delta)
        {
            if (effect == null) return;
            if (GlobalScale != Vector2.One)
            {
                GlobalScale = Vector2.One;
                UpdateProgressBarScale();
            }

            ProgressBar.GlobalPosition = GlobalPosition - _progressbarOffset;
            ProgressBar.Value = effect.Duration - effect.timer;
        }

        private void UpdateProgressBarScale()
        {
            if (_animatedSprite == null || ProgressBar == null) return;

            Texture2D texture = _animatedSprite.SpriteFrames.GetFrameTexture(_animatedSprite.Animation, _animatedSprite.Frame);
            Vector2 parentWorldSize = texture.GetSize() * _animatedSprite.GlobalScale;

            ProgressBar.Scale = parentWorldSize / ProgressBar.Size;
            _progressbarOffset = ProgressBar.Size * ProgressBar.Scale / 2f;
        }
    }
}