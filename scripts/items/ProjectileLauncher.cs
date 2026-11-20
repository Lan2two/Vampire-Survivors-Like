namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    [GlobalClass]
    public partial class ProjectileLauncher : Area2D, IWeapon, IUpgradable
    {
        [Export] public UpgradeManager upgradeManager;
        public Timer timer;
        private Marker2D ShootPoint;
        private Array<Node2D> enemiesInRange;
        [Export] public ProjectileData startingConfig;
        [Export] PackedScene Scene;
        public ProjectileData config;
        public Resource currentConfig => config;
        private bool shooting = true;

        public override void _Ready()
        {
            timer = GetNode<Timer>("Timer");
            ShootPoint = GetNode<Marker2D>("%ShootPoint");

            if (startingConfig == null)
            {
                GD.PrintErr("Launcher requires a BulletData resource.");
                return;
            }
            if (Scene == null)
            {
                GD.PrintErr("Launcher requires a Bullet Scene.");
                return;
            }
            if (upgradeManager == null)
            {
                GD.PrintErr("Launcher requires UpgradeManager.");
                return;
            }
            timer.Timeout += OnTimeout;
        }

        public override void _ExitTree()
        {
            if (timer != null)
            {
                timer.Timeout -= OnTimeout;
            }
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
                if (shooting)
                {
                    shooting = false;
                    timer.Start();
                    Use();
                }
            }
        }

        private void OnTimeout()
        {
            shooting = true;
        }

        public void Use()
        {
            if (enemiesInRange.Count > 0)
            {
                int sign = 1;
                for (int i = 0; i < config.GetStat("Burst").CalculateFinal(); i++)
                {
                    GetTree().CreateTimer(config.GetStat("BurstInterval").CalculateFinal() * i, false).Timeout += () =>
                    {
                        sign *= -1;
                        float spread = (float)GD.RandRange(0, config.GetStat("Spread").CalculateFinal() / 2);
                        Bullet projectileInstance = Scene.Instantiate<Bullet>();
                        projectileInstance.config = config;
                        projectileInstance.Fire();
                        projectileInstance.GlobalPosition = ShootPoint.GlobalPosition;
                        projectileInstance.GlobalRotation = ShootPoint.GlobalRotation + spread * sign;
                        ShootPoint.AddChild(projectileInstance);
                    };
                }
            }
        }
    }
}
