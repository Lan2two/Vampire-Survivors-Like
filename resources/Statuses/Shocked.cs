namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class Shocked : StatusEffect
    {
        [Export] private float InitialDamage { get; set; } = 5f;
        [Export] private float TickDamage { get; set; } = 2f;
        [Export] public override float TickInterval { get; set; } = 2f;
        public override string StatusName { get; set; } = "Shocked";

        private VelocityComponent velocity;
        private HealthComponent health;
        Attack attack = new Attack()
        {
            HitFlash = Colors.Yellow,
        };
        private float StoredAcceleration;

        public void Shock()
        {
            if (!IsInstanceValid(velocity) || !IsInstanceValid(health)) return;
            velocity.StopImmediately();
            health.TakeDamage(attack);
        }

        public override void OnInflict(Node node)
        {
            if (!IsInstanceValid(node)) return;
            velocity = node.GetComponent<VelocityComponent>();
            health = node.GetComponent<HealthComponent>();
            if (IsInstanceValid(velocity))
            {
                StoredAcceleration = velocity.acceleration;
            }
            velocity.acceleration = 3f;
            attack.Damage = InitialDamage;
            Shock();
        }
        public override void OnTick(Node target)
        {
            attack.Damage = TickDamage;
            Shock();
        }

        public override void OnExpire(Node target)
        {
            velocity.acceleration = StoredAcceleration;
        }
    }
}
