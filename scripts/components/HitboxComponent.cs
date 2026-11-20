using Godot;
using System;

[GlobalClass]
public partial class HitboxComponent : Area2D
{
    [Export] public HealthComponent healthComponent;

    public void TakeDamage(int damage)
    {
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(damage);
        }
        else
        {
            GD.PrintErr("HealthComponent not assigned to HitboxComponent.");
        }
    }
}
