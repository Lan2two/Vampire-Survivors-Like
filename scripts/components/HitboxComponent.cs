using Godot;
using System;

[GlobalClass]
public partial class HitboxComponent : Area2D
{
    [Export] public HealthComponent healthComponent;
    public bool IsInvincible { get; set; } = false;

    public void TakeDamage(Attack attackData)
    {
        if (healthComponent != null)
        {
            if (IsInvincible)
            {
                return;
            }
            healthComponent.TakeDamage(attackData);
        }
    }
}
