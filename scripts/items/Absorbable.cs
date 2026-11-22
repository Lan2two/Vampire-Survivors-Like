namespace SurvivorsGame
{
    using Godot;
    using System;

    [GlobalClass]
    public partial class Absorbable : RigidBody2D, IUpgradable, IPoolable
    {
        [Export] private DetectionComponent detectionComponent;
        [Export] private PathfindComponent pathfindComponent;
        [Export] private VelocityComponent velocityComponent;

        private AbsorbableData baseConfig;
        public Resource currentConfig { get; set; }

        public bool SawPlayer;
        public float detectionRangePercent;
        private CircleShape2D shape;
        private CollisionShape2D _collisionShape;

        protected bool _isActive = false;
        public Action<Absorbable> ReturnToPool { get; set; }

        public override void _Ready()
        {
            _collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");

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

        public virtual void OnSpawn()
        {
            _isActive = true;
            SawPlayer = false;
            LinearVelocity = Vector2.Zero;
            AngularVelocity = 0f;

            OnUpgrade();

            Show();
            SetDeferred(Node.PropertyName.ProcessMode, (int)ProcessModeEnum.Inherit);
            SetDeferred(RigidBody2D.PropertyName.Freeze, false);
            SetDeferred(RigidBody2D.PropertyName.ContactMonitor, true);

            if (_collisionShape != null)
            {
                _collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
            }
            if (detectionComponent != null)
            {
                detectionComponent.SetDeferred(Area2D.PropertyName.Monitoring, true);
            }
        }


        public virtual void OnDespawn()
        {
            _isActive = false;
            SawPlayer = false;

            Hide();
            SetDeferred(Node.PropertyName.ProcessMode, (int)ProcessModeEnum.Disabled);
            SetDeferred(RigidBody2D.PropertyName.Freeze, true);
            SetDeferred(RigidBody2D.PropertyName.ContactMonitor, false);

            if (_collisionShape != null)
            {
                _collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
            }
            if (detectionComponent != null)
            {
                detectionComponent.SetDeferred(Area2D.PropertyName.Monitoring, false);
            }
        }

        public void Despawn()
        {
            if (!_isActive) return;
            _isActive = false;

            if (ReturnToPool != null)
            {
                ReturnToPool.Invoke(this);
            }
            else
            {
                CallDeferred(Node.MethodName.QueueFree);
            }
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
            if (!_isActive) return;
            if (body is Player player)
            {
                OnPickup(player);
                Despawn();
            }
        }

        protected virtual void OnPickup(Player player)
        {
        }

        public virtual void SetAmount(float amount)
        {
        }
    }
}
