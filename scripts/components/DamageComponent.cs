using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class DamageComponent : Area2D
{
    [Export] int Damage = 10;
    [Export] float DamageInterval = .7f;

    private double damageTimer;

    public override void _PhysicsProcess(double delta)
    {
        damageTimer -= delta;
        if (damageTimer > 0)
        {
            return;
        }

        damageTimer = DamageInterval;

        Array<Area2D> areas = GetOverlappingAreas();
        foreach (Area2D area in areas)
        {
            if (area is HitboxComponent hitbox)
            {
                hitbox.TakeDamage(Damage);
            }
        }
    }
}
