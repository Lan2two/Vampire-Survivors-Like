using Godot;
using System;

[GlobalClass]
public partial class HitboxComponent : Area2D
{
    [Export] public HealthComponent healthComponent;
    public bool IsInvincible { get; set; } = false;

    public void TakeDamage(Attack attackData)
    {
        if (healthComponent == null)
        {
            return;
        }

        if (IsInvincible)
        {
            return;
        }
        healthComponent.TakeDamage(attackData);
    }
}
