namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class HitFlashComponent : Node
    {
        [Export] AnimatedSprite2D Sprite;
        [Export] HealthComponent healthComponent;
        [Export] Color flashColor = new Color(1, 0, 0); // Red flash color
        [Export] float hitFlashDuration = 0.1f;
        [Export] float healFlashDuartion = 0.3f;
        private Tween _flashTween;
        public override void _Ready()
        {
            if (Sprite == null)
            {
                GD.PrintErr("Sprite not assigned in HitFlashComponent.");
                return;
            }
            if (healthComponent == null)
            {
                GD.PrintErr("HealthComponent not assigned in HitFlashComponent.");
                return;
            }
            healthComponent.FinalDamageEvent += Flash;
        }

        public override void _ExitTree()
        {
            if (healthComponent != null)
            {
                healthComponent.FinalDamageEvent -= Flash;
            }
        }


        private void Flash(Attack attackData)
        {
            Color _targetColor;
            float _targetDuration;
            if (Sprite == null)
            {
                return;
            }
            if (_flashTween != null)
            {
                return;
            }
            if (attackData.HitFlash == default)
            {
                _targetColor = flashColor;
                _targetDuration = hitFlashDuration;
            }
            else
            {
                _targetColor = attackData.HitFlash;
                _targetDuration = attackData.FlashDuration;
            }
            _flashTween = CreateTween();
            _flashTween.TweenProperty(Sprite, "self_modulate", _targetColor, 0.1)
                .SetTrans(Tween.TransitionType.Sine)
                .SetEase(Tween.EaseType.InOut);
            _flashTween.TweenProperty(Sprite, "self_modulate", Colors.White, _targetDuration)
                .SetTrans(Tween.TransitionType.Sine)
                .SetEase(Tween.EaseType.InOut);

            _flashTween.Finished += () =>
            {
                if (_flashTween != null)
                {
                    _flashTween.Dispose();
                    _flashTween = null;
                }
            };
        }
    }
}
