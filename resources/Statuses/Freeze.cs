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
        private float StoredSpeed;
        private float StoredAcceleration;
        private Tween _flashTween;

        public void FreezeEnemy(Attack attack)
        {
            if (!IsInstanceValid(velocity) || !IsInstanceValid(health)) return;
            velocity.maxSpeed = 10f;
            velocity.acceleration = 2f;
            velocity.StopImmediately();
            health.TakeDamage(attack);
        }

        public void UnFreezeEnemy()
        {
            if (!IsInstanceValid(velocity) || !IsInstanceValid(health)) return;
            velocity.maxSpeed = StoredSpeed;
            velocity.acceleration = StoredAcceleration;
        }

        public override void OnInflict(Node node)
        {
            if (!IsInstanceValid(node)) return;
            velocity = node.GetComponent<VelocityComponent>();
            health = node.GetComponent<HealthComponent>();
            if (IsInstanceValid(velocity))
            {
                StoredSpeed = velocity.maxSpeed;
                StoredAcceleration = velocity.acceleration;
            }
            Attack attack = new Attack()
            {
                Damage = damage,
                HitFlash = Colors.Blue,
                FlashDuration = (float)Duration
            };
            FreezeEnemy(attack);
            if (node is Node2D node2D)
            {
                if (_flashTween != null)
                {
                    _flashTween.Dispose();
                    _flashTween = null;
                }
                _flashTween = node2D.CreateTween();
                Color InitalModulate = node2D.Modulate;
                _flashTween.TweenProperty(node, "modulate", FreezeColor, 0.1)
                 .SetTrans(Tween.TransitionType.Sine)
                 .SetEase(Tween.EaseType.InOut);
                _flashTween.TweenProperty(node, "modulate", InitalModulate, (float)Duration)
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

        public override void OnExpire(Node node)
        {
            if (!IsInstanceValid(node)) return;
            UnFreezeEnemy();
        }
    }
}
