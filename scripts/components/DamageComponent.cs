using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class DamageComponent : Area2D
{
    [Export] public float damage = 10f;
    [Export] public float knockback = 100f;
    [Export] public float DamageInterval = .7f;
    [Export] public bool SingleHit = true;

    private double damageTimer;
    private readonly HashSet<Area2D> hitboxes = new();
    public int UniqueHitboxesEntered;

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

            bool isNewHitbox = hitboxes.Add(area);
            if (isNewHitbox)
            {
                UniqueHitboxesEntered = hitboxes.Count;
            }

            if (SingleHit && !isNewHitbox)
            {
                continue;
            }
            Attack attack = new Attack
            {
                Damage = damage,
                GlobalPosition = this.GlobalPosition,
                KnockbackForce = knockback
            };
            hitbox.TakeDamage(attack);
        }
    }
}
