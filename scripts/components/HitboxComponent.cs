namespace SurvivorsGame
{
    using Godot;
    using System;

    [GlobalClass]
    public partial class HitboxComponent : Area2D
    {
        [Export] public HealthComponent healthComponent;
        public bool IsInvincible { get; set; } = false;

        public void TakeDamage(Attack attackData)
        {
            if (healthComponent == null || IsInvincible)
            {
                return;
            }

            HealthComponent targetHealth = healthComponent;
            Callable.From(() =>
            {
                if (!GodotObject.IsInstanceValid(this)
                    || !GodotObject.IsInstanceValid(targetHealth)
                    || IsInvincible)
                {
                    return;
                }

                targetHealth.TakeDamage(attackData);
            }).CallDeferred();
        }
    }
}
