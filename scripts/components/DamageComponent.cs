using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class DamageComponent : Area2D
{
    [Export] int Damage = 10;
    [Export] float DamageInterval = .7f;
    [Export] bool SingleHit = false;

    private double damageTimer;
    private readonly HashSet<Area2D> hitAreas = new();

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
            if (area is not HitboxComponent hitbox)
            {
                continue;
            }

            if (SingleHit && hitAreas.Contains(area))
            {
                continue;
            }

            if (SingleHit)
            {
                hitAreas.Add(area);
            }

            hitbox.TakeDamage(Damage);
        }
    }
}
