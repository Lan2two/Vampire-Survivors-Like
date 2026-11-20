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
        private readonly HashSet<Area2D> hurtboxes = new();
        private readonly HashSet<Area2D> damagedHurtboxes = new();
        public int UniqueHurtboxesEntered;

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
            foreach (Area2D area in new List<Area2D>(hurtboxes))
            {
                ApplyDamage(area);
            }
        }



        private void ApplyDamage(Area2D area)
        {
            if (area is not HurtboxComponent hurtbox)
            {
                return;
            }

            if (SingleHit && damagedHurtboxes.Contains(area))
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
                damagedHurtboxes.Add(area);
            }

            hurtbox.TakeDamage(attack);
        }

        public void ClearHash()
        {
            hurtboxes.Clear();
            damagedHurtboxes.Clear();
            UniqueHurtboxesEntered = hurtboxes.Count;
        }

        private void OnAreaEntered(Area2D area)
        {
            if (area is not HurtboxComponent)
            {
                return;
            }

            if (SingleHit)
            {
                ApplyDamage(area);
                return;
            }

            bool isNewHurtbox = hurtboxes.Add(area);
            if (isNewHurtbox)
            {
                UniqueHurtboxesEntered = hurtboxes.Count;
            }
        }

        private void OnAreaExited(Area2D area)
        {
            if (SingleHit)
            {
                return;
            }
            if (area is not HurtboxComponent)
            {
                return;
            }

            hurtboxes.Remove(area);
            damagedHurtboxes.Remove(area);
            UniqueHurtboxesEntered = hurtboxes.Count;
        }
    }
}
