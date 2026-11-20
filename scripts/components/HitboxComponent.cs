using Godot;
using System;

[GlobalClass]
public partial class HitboxComponent : Area2D
{
    [Export] public HealthComponent healthComponent;

    public void TakeDamage(Attack attackData)
    {
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(attackData);
        }
    }
}
