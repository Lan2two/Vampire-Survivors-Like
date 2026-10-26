namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    [GlobalClass]
    public partial class GunComponent : Area2D
    {
        UpgradeManager upgradeManager;
        Timer timer;
        Marker2D ShootPoint;
        Array<Node2D> enemiesInRange;
        [Export] BulletData bulletConfig;
        [Export] PackedScene BULLET;


        public override void _Ready()
        {
            timer = GetNode<Timer>("Timer");
            ShootPoint = GetNode<Marker2D>("%ShootPoint");
            upgradeManager = GetParent().GetNode<UpgradeManager>("UpgradeManager");

            if (bulletConfig == null)
            {
                GD.PrintErr("GunComponent requires a BulletData resource.");
                return;
            }
            if (BULLET == null)
            {
                GD.PrintErr("GunComponent requires a Bullet Scene.");
                return;
            }

            timer.Stop();
            timer.WaitTime = GetFiringConfig().GunCooldown;
            timer.Timeout += Shoot;
            timer.Start();
        }

        public override void _PhysicsProcess(double delta)
        {
            enemiesInRange = GetOverlappingBodies();
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
                LookAt(closestEnemy.GlobalPosition);
            }
        }

        private void Shoot()
        {
            timer.WaitTime = GetFiringConfig().GunCooldown;

            if (enemiesInRange.Count > 0)
            {
                Bullet newBullet = BULLET.Instantiate<Bullet>();
                newBullet.Use(upgradeManager);
                newBullet.GlobalPosition = ShootPoint.GlobalPosition;
                newBullet.GlobalRotation = ShootPoint.GlobalRotation;
                ShootPoint.AddChild(newBullet);
            }
        }

        private BulletData GetFiringConfig()
        {
            BulletData firingConfig = (BulletData)bulletConfig.Duplicate();
            upgradeManager.ApplyUpgrades(firingConfig, UpgradeType.Bullet);
            return firingConfig;
        }
    }
}
