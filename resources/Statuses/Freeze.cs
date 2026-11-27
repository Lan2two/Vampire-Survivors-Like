namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class Freeze : StatusEffect
    {
        [Export] private float damage { get; set; } = 5f;
        public override string StatusName { get; set; } = "Freeze";

        private VelocityComponent velocity;
        private HealthComponent health;
        private float StoredSpeed;

        public virtual void FreezeEnemy(Attack attack)
        {
            if (!IsInstanceValid(velocity) || !IsInstanceValid(health)) return;
            StoredSpeed = velocity.maxSpeed;
            velocity.maxSpeed = 0f;
            velocity.StopImmediately();
            health.TakeDamage(attack);
        }

        public virtual void UnFreezeEnemy()
        {
            if (!IsInstanceValid(velocity) || !IsInstanceValid(health)) return;
            velocity.maxSpeed = StoredSpeed;
        }

        public override void OnInflict(Node node)
        {
            if (!IsInstanceValid(node)) return;
            velocity = node.GetComponent<VelocityComponent>();
            health = node.GetComponent<HealthComponent>();
            Attack attack = new Attack()
            {
                Damage = damage,
                HitFlash = Colors.Blue,
                FlashDuration = (float)Duration
            };
            FreezeEnemy(attack);
        }

        public override void OnExpire(Node node)
        {
            if (!IsInstanceValid(node)) return;
            UnFreezeEnemy();
        }
    }
}
