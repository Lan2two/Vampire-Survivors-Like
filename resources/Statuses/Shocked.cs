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
        private Attack attack = new Attack()
        {
            HitFlash = Colors.Yellow,
        };

        public void Shock()
        {
            if (IsInstanceValid(velocity))
            {
                velocity.StopImmediately();
            }
            if (IsInstanceValid(health))
            {
                health.TakeDamage(attack);
            }
        }

        public override void OnInflict(Node node)
        {
            if (!IsInstanceValid(node)) return;
            velocity = node.GetComponent<VelocityComponent>();
            health = node.GetComponent<HealthComponent>();

            if (IsInstanceValid(velocity))
            {
                velocity.SetModifier(StatusName, speedMult: 1f, accelMult: 0.075f);
            }

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
            if (IsInstanceValid(velocity))
            {
                velocity.RemoveModifier(StatusName);
            }
        }
    }
}
