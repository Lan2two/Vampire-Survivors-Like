namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    [GlobalClass]
    public partial class ProjectileLauncher : Area2D, IWeapon, IUpgradable
    {
        [ExportGroup("Projectile Launcher")]
        [Export] public ProjectileData startingConfig;
        [Export] PackedScene Scene;
        [Export] Texture2D texture2D;
        public Timer timer;
        private Sprite2D sprite2D;
        private Marker2D shootPoint;
        private Marker2D weaponPivot;
        private Array<Node2D> enemiesInRange;
        public ProjectileData config;
        public Resource currentConfig => config;
        public UpgradeManager upgradeManager;
        private bool shooting = true;

        public override void _Ready()
        {
            timer = GetNode<Timer>("Timer");
            shootPoint = GetNode<Marker2D>("%ShootPoint");
            sprite2D = GetNodeOrNull<Sprite2D>("Sprite2D");
            weaponPivot = GetNodeOrNull<Marker2D>("WeaponPivot");

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
            if (weaponPivot == null)
            {
                GD.PrintErr("WeaponPivot not found in Projectile Launcher.");
                return;
            }
            if (texture2D != null)
            {
                sprite2D.Texture = texture2D;
            }
            timer.Timeout += OnTimeout;
            CallDeferred(MethodName.Onready);
        }

        private void Onready()
        {
            upgradeManager = this.GetPlayer().upgradeManager;
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
                        weaponPivot.GlobalRotation += spread * sign;
                        Bullet projectileInstance = Scene.Instantiate<Bullet>();
                        projectileInstance.config = config;
                        projectileInstance.Fire();
                        projectileInstance.GlobalPosition = shootPoint.GlobalPosition;
                        projectileInstance.GlobalRotation = shootPoint.GlobalRotation;
                        shootPoint.AddChild(projectileInstance);
                    };
                }
            }
        }
    }
}
