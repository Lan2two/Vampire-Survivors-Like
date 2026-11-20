namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;
    using System;
    using System.Collections.Generic;

    [GlobalClass]
    public partial class AttackComponent : Area2D
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
            Monitorable = false;
            AreaEntered += OnAreaEntered;
            AreaExited += OnAreaExited;
        }

        public void SetAttackMonitoring(bool enabled)
        {
            Monitoring = enabled;
        }

        public override void _ExitTree()
        {
            AreaEntered -= OnAreaEntered;
            AreaExited -= OnAreaExited;
        }

        public override void _PhysicsProcess(double delta)
        {
            if (!Monitoring)
            {
                return;
            }

            if (SingleHit)
            {
                Array<Area2D> overlappingAreas = GetOverlappingAreas();
                foreach (Area2D area in overlappingAreas)
                {
                    ApplyDamage(area);
                }
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
                ApplyDamage(area);
            }
        }



        private void ApplyDamage(Area2D area)
        {
            if (area is not HitboxComponent hitbox)
            {
                return;
            }

            if (SingleHit && damagedHitboxes.Contains(area))
            {
                return;
            }

            Attack attack = new Attack
            {
                Damage = damage,
                GlobalPosition = GlobalPosition,
                KnockbackForce = knockback
            };

            if (SingleHit)
            {
                damagedHitboxes.Add(area);
            }

            hitbox.TakeDamage(attack);
        }

        public void ClearHash()
        {
            hitboxes.Clear();
            damagedHitboxes.Clear();
            UniqueHitboxesEntered = hitboxes.Count;
        }

        private void OnAreaEntered(Area2D area)
        {
            if (area is not HitboxComponent)
            {
                return;
            }

            if (SingleHit)
            {
                ApplyDamage(area);
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
            if (SingleHit)
            {
                return;
            }
            if (area is not HitboxComponent)
            {
                return;
            }

            hitboxes.Remove(area);
            damagedHitboxes.Remove(area);
            UniqueHitboxesEntered = hitboxes.Count;
        }
    }
}
