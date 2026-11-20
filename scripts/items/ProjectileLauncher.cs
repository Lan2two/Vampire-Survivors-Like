namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    [GlobalClass]
    public partial class ProjectileLauncher : Area2D, IWeapon, IUpgradable
    {
        [ExportGroup("Projectile Launcher")]
        [Export] public ProjectileData startingConfig;
        [Export] private PackedScene Scene;
        [Export] private Texture2D texture2D;

        public Timer timer;
        private Sprite2D sprite2D;
        public CircleShape2D detectionRange;
        private Marker2D shootPoint;
        private Array<Node2D> enemiesInRange;

        public Resource currentConfig { get; set; }
        public UpgradeManager upgradeManager;
        private bool shooting = true;

        public override void _Ready()
        {
            timer = GetNodeOrNull<Timer>("Timer");
            shootPoint = GetNodeOrNull<Marker2D>("%ShootPoint");
            sprite2D = GetNodeOrNull<Sprite2D>("Sprite2D");

            var collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
            if (collisionShape?.Shape is CircleShape2D baseShape)
            {
                detectionRange = baseShape.Duplicate() as CircleShape2D;
                collisionShape.Shape = detectionRange;
            }

            if (startingConfig == null)
            {
                GD.PrintErr("Launcher requires a ProjectileData resource.");
                return;
            }
            if (Scene == null)
            {
                GD.PrintErr("Launcher requires a Projectile Scene.");
                return;
            }
            if (sprite2D == null)
            {
                GD.PrintErr("Sprite2D not found in Projectile Launcher.");
                return;
            }
            if (detectionRange == null)
            {
                GD.PrintErr("CircleShape2D not found in Projectile Launcher.");
                return;
            }

            if (texture2D != null)
            {
                sprite2D.Texture = texture2D;
            }

            startingConfig = (ProjectileData)startingConfig.Duplicate(true);
            startingConfig.DetectionRangeBase = detectionRange.Radius;
            currentConfig = (ProjectileData)startingConfig.Duplicate(true);

            if (timer != null)
            {
                timer.WaitTime = startingConfig.GetStat("ProjectileCooldown").CalculateFinal();
                timer.Timeout += OnTimeout;
            }

            CallDeferred(MethodName.Onready);
        }

        private void Onready()
        {
            upgradeManager = this.GetPlayer()?.upgradeManager;
        }

        public override void _ExitTree()
        {
            if (IsInstanceValid(timer))
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
                if (!IsInstanceValid(enemy)) continue;

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
            if (enemiesInRange == null || enemiesInRange.Count == 0)
            {
                return;
            }

            if (currentConfig is not ProjectileData activeConfig)
            {
                return;
            }

            int burstCount = Mathf.Max(1, (int)activeConfig.GetStat("Burst").CalculateFinal());
            float burstInterval = activeConfig.GetStat("BurstInterval").CalculateFinal();
            float halfSpreadRad = Mathf.DegToRad(activeConfig.GetStat("Spread").CalculateFinal() / 2f);

            int sign = 1;
            for (int i = 0; i < burstCount; i++)
            {
                int currentSign = sign;
                sign *= -1;
                float delay = burstInterval * i;

                if (delay <= 0f)
                {
                    SpawnBullet(activeConfig, halfSpreadRad, currentSign);
                }
                else
                {
                    GetTree().CreateTimer(delay, false).Timeout += () =>
                    {
                        if (!IsInstanceValid(this) || !IsInstanceValid(shootPoint)) return;
                        SpawnBullet(activeConfig, halfSpreadRad, currentSign);
                    };
                }
            }
        }

        private void SpawnBullet(ProjectileData activeConfig, float halfSpreadRad, int sign)
        {
            float spread = (float)GD.RandRange(0.0, halfSpreadRad);
            Bullet projectileInstance = Scene.Instantiate<Bullet>();
            projectileInstance.config = activeConfig;
            projectileInstance.GlobalPosition = shootPoint.GlobalPosition;
            projectileInstance.GlobalRotation = shootPoint.GlobalRotation + (spread * sign);

            Node spawnParent = GetTree().CurrentScene ?? GetParent();
            spawnParent.AddChild(projectileInstance);
            projectileInstance.Fire();
        }

        public void ApplyStrategy(UpgradeType upgradeType)
        {
            if (startingConfig == null) return;

            currentConfig = (ProjectileData)startingConfig.Duplicate(true);

            if (upgradeManager == null)
            {
                upgradeManager = this.GetPlayer()?.upgradeManager;
            }

            upgradeManager?.ApplyUpgrades(this, upgradeType);

            if (currentConfig is ProjectileData activeConfig)
            {
                detectionRange.Radius = activeConfig.GetStat("DetectionRange").CalculateFinal();
                timer.WaitTime = Mathf.Max(0.05f, activeConfig.GetStat("ProjectileCooldown").CalculateFinal());
            }
        }
    }
}
