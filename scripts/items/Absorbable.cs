namespace SurvivorsGame
{
    using Godot;
    [GlobalClass]
    public partial class Absorbable : RigidBody2D, IUpgradable
    {
        [Export] private DetectionComponent detectionComponent;
        [Export] private PathfindComponent pathfindComponent;
        [Export] private VelocityComponent velocityComponent;
        private AbsorbableData baseConfig;
        private AbsorbableData config;
        public Resource currentConfig => config;
        public bool SawPlayer;
        public float detectionRangePercent;
        private UpgradeManager upgradeManager;
        CircleShape2D shape;
        public override void _Ready()
        {
            if (detectionComponent == null)
            {
                GD.PrintErr("DetectionComponent node not found in Absorbable");
                return;
            }
            if (pathfindComponent == null)
            {
                GD.PrintErr("PathfindComponent not found in Absorbable");
                return;
            }
            upgradeManager = this.GetPlayer().upgradeManager;
            shape = detectionComponent.shape.Shape as CircleShape2D;
            baseConfig = new AbsorbableData()
            {
                RadiusBase = shape.Radius,
                MoveSpeedBase = velocityComponent.maxSpeed
            };
            OnUpgrade();
            BodyEntered += OnBodyEntered;
            EventBus.AbsorbableUpgrade += OnUpgrade;
        }

        public override void _ExitTree()
        {
            BodyEntered -= OnBodyEntered;
            EventBus.AbsorbableUpgrade -= OnUpgrade;
        }

        private void OnUpgrade()
        {
            config = (AbsorbableData)baseConfig.Duplicate();
            upgradeManager.ApplyUpgrades(this, UpgradeType.Absorbable);
            shape.Radius = config.GetStat("Radius").CalculateFinal();
            velocityComponent.maxSpeed = config.GetStat("MoveSpeed").CalculateFinal();
        }

        public override void _PhysicsProcess(double delta)
        {
            if (detectionComponent.IsPlayerInRange() || SawPlayer)
            {
                pathfindComponent.PathfindToPlayer();
            }
        }

        private void OnBodyEntered(Node body)
        {
            if (body is Player)
            {
                CallDeferred(Node.MethodName.QueueFree);
            }
        }

        public virtual void SetAmount(float amount)
        { }
    }
}