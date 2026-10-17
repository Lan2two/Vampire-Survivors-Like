namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class KnockbackComponent : Node
    {
        [Export] HealthComponent healthComponent;
        [Export] VelocityComponent velocityComponent;
        [Export] RigidBody2D characterBody;
        public override void _Ready()
        {
            if (velocityComponent == null)
            {
                GD.PrintErr("VelocityComponent unset in Knockback");
                return;
            }
            healthComponent.Damage += TakeKnockback;
        }
        public override void _ExitTree()
        {
            if (healthComponent != null)
            {
                healthComponent.Damage -= TakeKnockback;
            }
        }

        private void TakeKnockback(Attack attackData)
        {
            if (velocityComponent == null)
            {
                return;
            }

            Vector2 pushDirection = (characterBody.GlobalPosition - attackData.GlobalPosition).Normalized();
            Vector2 knockbackVelocity = pushDirection * attackData.KnockbackForce;

            velocityComponent.ApplyVelocityChange(knockbackVelocity);
        }

    }
}
