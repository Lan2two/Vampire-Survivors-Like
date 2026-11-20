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
    private readonly HashSet<Area2D> damagedHitboxes = new();
    public int UniqueHitboxesEntered;

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;
    }

    public override void _ExitTree()
    {
        AreaEntered -= OnAreaEntered;
        AreaExited -= OnAreaExited;
    }

    public override void _Process(double delta)
    {
        if (!Monitoring)
        {
            return;
        }

        damageTimer -= delta;
        if (damageTimer > 0)
        {
            return;
        }

        damageTimer = DamageInterval;

        foreach (Area2D area in new List<Area2D>(hitboxes))
        {
            if (area is not HitboxComponent hitbox)
            {
                continue;
            }

            if (SingleHit && damagedHitboxes.Contains(area))
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

            if (SingleHit)
            {
                damagedHitboxes.Add(area);
            }
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is not HitboxComponent)
        {
            return;
        }

        if (SingleHit && damagedHitboxes.Contains(area))
        {
            return;
        }

        bool isNewHitbox = hitboxes.Add(area);
        if (isNewHitbox)
        {
            UniqueHitboxesEntered = hitboxes.Count;
        }
    }

    private void OnAreaExited(Area2D area)
    {
        if (area is not HitboxComponent)
        {
            return;
        }

        hitboxes.Remove(area);
        damagedHitboxes.Remove(area);
        UniqueHitboxesEntered = hitboxes.Count;
    }
}
