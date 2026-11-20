namespace SurvivorsGame
{
    using System.Linq;

    using Godot;
    using Godot.Collections;

    public partial class RedirectBullet : Area2D
    {
        [Export] AttackComponent attackComponent;
        [Export] Bullet bullet;
        public bool Redirect;

        public override void _Ready()
        {
            if (attackComponent == null)
            {
                GD.PrintErr("AttackComponent unset in Redirect Bullet");
            }
            if (bullet == null)
            {
                GD.PrintErr("Bullet unset in Redirect Bullet");
            }
            attackComponent.DamageApplied += OnDamageApplied;
        }

        public override void _ExitTree()
        {
            if (attackComponent != null)
            {
                attackComponent.DamageApplied -= OnDamageApplied;
            }
        }

        private void OnDamageApplied()
        {
            if (!Redirect)
            {
                return;
            }
            Array<Node2D> enemiesInRange = GetOverlappingBodies();
            foreach (Area2D area in attackComponent.damagedHurtboxes)
            {
                if (IsInstanceValid(area))
                {
                    Node2D body = area.GetParent() as Node2D;
                    if (body != null)
                    {
                        enemiesInRange.Remove(body);
                    }
                }
            }
            if (enemiesInRange.Count == 0)
            {
                return;
            }
            Node2D closestEnemy = null;
            float closestDistSq = float.MaxValue;
            foreach (Node2D enemy in enemiesInRange)
            {
                float distSq = GlobalPosition.DistanceSquaredTo(enemy.GlobalPosition);
                if (distSq < closestDistSq)
                {
                    closestDistSq = distSq;
                    closestEnemy = enemy;
                }
            }
            if (closestEnemy != null)
            {
                bullet.LookAt(closestEnemy.GlobalPosition);
            }
        }

    }
}