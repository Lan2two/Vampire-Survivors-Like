namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    [GlobalClass]
    public partial class ProjectileLauncher : Area2D, IWeapon, IUpgradable, IPoolPrewarmer
    {
        [ExportGroup("Projectile Launcher")]
        [Export] public ProjectileData startingConfig;
        [Export] private PackedScene Scene;
        [Export] private Texture2D texture2D;
        [Export] private bool LookAtEnemy = true;
        [Export] private bool AlwaysFire = false;

        public Timer timer;
        private Sprite2D sprite2D;
        public CircleShape2D detectionRange;
        private Marker2D WeaponPivot;
        private Array<Marker2D> shootPoints = new();
        private Array<Node2D> enemiesInRange;

        private ObjectPool<Bullet> _bulletPool;
        public Resource currentConfig { get; set; }
        public UpgradeManager upgradeManager;
        private bool shooting = true;

        private const int InitialPoolCount = 10;

        public override void _Ready()
        {
            timer = GetNodeOrNull<Timer>("Timer");
            sprite2D = GetNodeOrNull<Sprite2D>("Sprite2D");
            WeaponPivot = GetNodeOrNull<Marker2D>("WeaponPivot");

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
            if (WeaponPivot == null)
            {
                GD.PrintErr("WeaponPivot not found in Projectile Launcher.");
                return;
            }

            foreach (Node node in WeaponPivot.GetChildren())
            {
                if (node is Marker2D marker)
                {
                    shootPoints.Add(marker);
                }
            }

            if (texture2D != null)
            {
                sprite2D.Texture = texture2D;
            }

            startingConfig = startingConfig.Clone();
            startingConfig.DetectionRangeBase = detectionRange.Radius;
            currentConfig = startingConfig.Clone();

            if (timer != null)
            {
                timer.WaitTime = startingConfig.GetStat("ProjectileCooldown").CalculateFinal();
                timer.Timeout += OnTimeout;
            }

            CallDeferred(MethodName.Onready);
        }

        private void Onready()
        {
            _bulletPool = new ObjectPool<Bullet>(Scene, GetTree().CurrentScene, InitialPoolCount);
            upgradeManager = this.GetPlayer()?.upgradeManager;
        }

        public override void _ExitTree()
        {
            if (IsInstanceValid(timer))
            {
                timer.Timeout -= OnTimeout;
            }
            _bulletPool?.Clear();
        }

        public override void _PhysicsProcess(double delta)
        {
            if (LookAtEnemy)
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
                    AttempShoot();
                }
                else if (AlwaysFire)
                {
                    AttempShoot();
                }
            }
            else if (AlwaysFire || GetOverlappingBodies().Count > 0)
            {
                AttempShoot();
            }

        }

        private void AttempShoot()
        {
            if (shooting)
            {
                shooting = false;
                timer.Start();
                Use();
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
            float halfSpread = activeConfig.GetStat("Spread").CalculateFinal() / 2f;

            int sign = 1;
            for (int i = 0; i < burstCount; i++)
            {
                foreach (Marker2D shootpoint in shootPoints)
                {
                    int currentSign = sign;
                    sign *= -1;
                    float delay = burstInterval * i;

                    if (delay <= 0f)
                    {
                        SpawnBullet(activeConfig, halfSpread, currentSign, shootpoint);
                    }
                    else
                    {
                        GetTree().CreateTimer(delay, false).Timeout += () =>
                        {

                            if (!IsInstanceValid(this) || !IsInstanceValid(shootpoint)) return;
                            SpawnBullet(activeConfig, halfSpread, currentSign, shootpoint);

                        };
                    }
                }
            }
        }

        private void SpawnBullet(ProjectileData activeConfig, float halfSpread, int sign, Marker2D shootPoint)
        {
            if (_bulletPool == null) return;

            float spread = (float)GD.RandRange(0.0, halfSpread);
            //GD.Print($"{activeConfig.GetStat("Spread").CalculateFinal()}, {spread}");
            Bullet projectileInstance = _bulletPool.Get();
            projectileInstance.config = activeConfig;
            projectileInstance.GlobalPosition = shootPoint.GlobalPosition;
            projectileInstance.GlobalRotation = shootPoint.GlobalRotation + (spread * sign);
            projectileInstance.ReturnToPool = _bulletPool.Return;
            projectileInstance.OnSpawn();
        }

        public void ApplyStrategy(UpgradeType upgradeType)
        {
            if (startingConfig == null) return;

            currentConfig = startingConfig.Clone();

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

        System.Collections.Generic.IEnumerable<(PackedScene scene, int count)> IPoolPrewarmer.GetPoolRequirements()
        {
            if (Scene != null) yield return (Scene, InitialPoolCount);
        }
    }

}
