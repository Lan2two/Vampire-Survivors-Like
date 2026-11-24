namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class KnockbackComponent : Node
    {
        [Export] private HealthComponent healthComponent;
        [Export] private VelocityComponent velocityComponent;
        [Export] private RigidBody2D characterBody;
        [Export] private double resetDuration = 10.0;

        private double timer;
        private int knockbackCount;

        public override void _Ready()
        {
            if (healthComponent == null)
            {
                GD.PrintErr("HealthComponent unset in KnockbackComponent");
                return;
            }
            if (velocityComponent == null)
            {
                GD.PrintErr("VelocityComponent unset in KnockbackComponent");
                return;
            }
            if (characterBody == null)
            {
                GD.PrintErr("CharacterBody unset in KnockbackComponent");
                return;
            }

            healthComponent.FinalDamageEvent += TakeKnockback;
        }

        public override void _ExitTree()
        {
            if (healthComponent != null)
            {
                healthComponent.FinalDamageEvent -= TakeKnockback;
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            if (knockbackCount > 0)
            {
                timer -= delta;
                if (timer <= 0)
                {
                    Reset();
                }
            }
        }

        public void Reset()
        {
            timer = 0;
            knockbackCount = 0;
        }

        private void TakeKnockback(Attack attackData)
        {
            if (velocityComponent == null || characterBody == null)
            {
                return;
            }

            timer = resetDuration;

            Vector2 pushDirection = (characterBody.GlobalPosition - attackData.GlobalPosition).Normalized();
            float diminishingMultiplier = 1f / (knockbackCount * 0.5f + 1f);
            Vector2 knockbackVelocity = pushDirection * attackData.KnockbackForce * diminishingMultiplier;

            knockbackCount++;
            velocityComponent.ApplyVelocityChange(knockbackVelocity);
        }
    }
}
