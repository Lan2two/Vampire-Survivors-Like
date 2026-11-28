namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class Freeze : StatusEffect
    {
        [Export] private float damage { get; set; } = 5f;
        public override string StatusName { get; set; } = "Freeze";
        private Color FreezeColor = Colors.Blue;

        private VelocityComponent velocity;
        private HealthComponent health;
        private Color _initialModulate;
        private bool _isActive = false;
        private Tween _flashTween;

        public override void OnInflict(Node node)
        {
            if (!IsInstanceValid(node)) return;
            velocity = node.GetComponent<VelocityComponent>();
            health = node.GetComponent<HealthComponent>();

            if (IsInstanceValid(velocity))
            {
                velocity.SetModifier(StatusName, speedMult: 0.1f, accelMult: 0.05f);
                velocity.StopImmediately();
            }

            if (IsInstanceValid(health))
            {
                Attack attack = new Attack()
                {
                    Damage = damage,
                    HitFlash = Colors.Blue,
                    FlashDuration = (float)Duration
                };
                health.TakeDamage(attack);
            }

            if (node is Node2D node2D)
            {
                if (!_isActive)
                {
                    _initialModulate = node2D.Modulate;
                    _isActive = true;
                }

                if (_flashTween != null && IsInstanceValid(_flashTween))
                {
                    _flashTween.Kill();
                }

                _flashTween = node2D.CreateTween();
                _flashTween.TweenProperty(node2D, "modulate", FreezeColor, 0.1)
                    .SetTrans(Tween.TransitionType.Sine)
                    .SetEase(Tween.EaseType.InOut);
                _flashTween.TweenProperty(node2D, "modulate", _initialModulate, (float)Duration)
                    .SetTrans(Tween.TransitionType.Sine)
                    .SetEase(Tween.EaseType.InOut);
            }
        }

        public override void OnExpire(Node node)
        {
            _isActive = false;
            if (!IsInstanceValid(node)) return;

            if (IsInstanceValid(velocity))
            {
                velocity.RemoveModifier(StatusName);
            }

            if (_flashTween != null && IsInstanceValid(_flashTween))
            {
                _flashTween.Kill();
            }

            if (node is Node2D node2D)
            {
                node2D.Modulate = _initialModulate;
            }
        }
    }
}
