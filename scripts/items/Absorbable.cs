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
        public Resource currentConfig { get; set; }

        public bool SawPlayer;
        public float detectionRangePercent;
        private CircleShape2D shape;

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
            if (velocityComponent == null)
            {
                GD.PrintErr("VelocityComponent not found in Absorbable");
                return;
            }

            shape = detectionComponent.shape.Shape.Duplicate() as CircleShape2D;
            if (shape == null)
            {
                GD.PrintErr("CircleShape2D not found in Absorbable detection component");
                return;
            }

            detectionComponent.shape.Shape = shape;

            baseConfig = new AbsorbableData()
            {
                RadiusBase = shape.Radius,
                MoveSpeedBase = velocityComponent.maxSpeed
            };

            if (currentConfig == null)
            {
                currentConfig = baseConfig;
            }

            OnUpgrade();

            BodyEntered += OnBodyEntered;
            EventBus.AbsorbableUpgrade += OnUpgrade;
        }

        public override void _ExitTree()
        {
            BodyEntered -= OnBodyEntered;
            EventBus.AbsorbableUpgrade -= OnUpgrade;
        }

        public void OnUpgrade()
        {
            if (UpgradeManager.Instance == null || currentConfig == null)
            {
                return;
            }

            UpgradeManager.Instance.ApplyUpgrades(this, UpgradeType.Absorbable);

            if (currentConfig is AbsorbableData activeConfig)
            {
                shape.Radius = activeConfig.GetStat("Radius").CalculateFinal();
                velocityComponent.maxSpeed = activeConfig.GetStat("MoveSpeed").CalculateFinal();
            }
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
        {
        }
    }
}
