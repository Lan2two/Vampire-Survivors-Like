namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    [GlobalClass]
    public partial class Gun : Area2D, IWeapon, IUpgradable
    {
        [Export] private UpgradeManager upgradeManager;
        Timer timer;
        Marker2D ShootPoint;
        Array<Node2D> enemiesInRange;
        [Export] BulletData startingConfig;
        [Export] PackedScene BULLET;
        private BulletData config;
        public Resource currentConfig => config;

        public override void _Ready()
        {
            timer = GetNode<Timer>("Timer");
            ShootPoint = GetNode<Marker2D>("%ShootPoint");

            if (startingConfig == null)
            {
                GD.PrintErr("Gun requires a BulletData resource.");
                return;
            }
            if (BULLET == null)
            {
                GD.PrintErr("Gun requires a Bullet Scene.");
                return;
            }
            if (upgradeManager == null)
            {
                GD.PrintErr("Gun requires UpgradeManager.");
                return;
            }
            OnUpgrade();
            timer.Stop();
            timer.Start();
            timer.Timeout += Use;
            EventBus.BulletUpgrade += OnUpgrade;
        }

        public override void _ExitTree()
        {
            if (timer != null)
            {
                timer.Timeout -= Use;
            }
            EventBus.BulletUpgrade -= OnUpgrade;
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

        public void Use()
        {
            if (enemiesInRange.Count > 0)
            {
                Bullet newBullet = BULLET.Instantiate<Bullet>();
                newBullet.config = config;
                newBullet.Fire();
                newBullet.GlobalPosition = ShootPoint.GlobalPosition;
                newBullet.GlobalRotation = ShootPoint.GlobalRotation;
                ShootPoint.AddChild(newBullet);
            }
        }

        private void OnUpgrade()
        {
            config = (BulletData)startingConfig.Duplicate();
            upgradeManager.ApplyUpgrades(this, UpgradeType.Bullet);
            timer.WaitTime = config.GunCooldown;
        }
    }
}
