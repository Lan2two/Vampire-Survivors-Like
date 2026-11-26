namespace SurvivorsGame
{
    using Godot;
    using System;
    using System.Collections.Generic;

    [GlobalClass]
    public partial class Absorbable : RigidBody2D, IUpgradable, IPoolable
    {
        [Export] private float MergeRadius = 25f;
        [Export] private float CollectRadius = 12f;
        [Export] private float BasePickupRadius = 10f;
        [Export] private PathfindComponent pathfindComponent;
        [Export] private VelocityComponent velocityComponent;

        private AbsorbableData baseConfig;
        public Resource currentConfig { get; set; }

        public bool SawPlayer;
        public float detectionRangePercent;
        private float _pickupRadius;
        private CollisionShape2D _collisionShape;
        private Player _player;

        protected bool _isActive = false;
        public Action<Absorbable> ReturnToPool { get; set; }

        public static readonly List<Absorbable> _activeAbsorbables = new();

        public override void _Ready()
        {
            _collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");

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

            _pickupRadius = BasePickupRadius;

            baseConfig = new AbsorbableData()
            {
                RadiusBase = BasePickupRadius,
                MoveSpeedBase = velocityComponent.maxSpeed
            };

            currentConfig ??= baseConfig;

            OnUpgrade();

            BodyEntered += OnBodyEntered;
            EventBus.AbsorbableUpgrade += OnUpgrade;
        }

        public override void _ExitTree()
        {
            _activeAbsorbables.Remove(this);
            BodyEntered -= OnBodyEntered;
            EventBus.AbsorbableUpgrade -= OnUpgrade;
        }

        public virtual void OnSpawn()
        {
            if (Merge())
            {
                ReturnToPool?.Invoke(this);
                return;
            }
            SawPlayer = false;
            _isActive = true;
            if (!_activeAbsorbables.Contains(this))
            {
                _activeAbsorbables.Add(this);
            }

            LinearVelocity = Vector2.Zero;
            AngularVelocity = 0f;

            OnUpgrade();

            Show();
            ProcessMode = ProcessModeEnum.Inherit;
            SetDeferred(RigidBody2D.PropertyName.Freeze, false);
            SetDeferred(RigidBody2D.PropertyName.ContactMonitor, true);

            if (_collisionShape != null)
            {
                _collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
            }
        }

        public virtual void OnDespawn()
        {
            _isActive = false;
            _activeAbsorbables.Remove(this);
            SawPlayer = false;

            Hide();
            SetDeferred(Node.PropertyName.ProcessMode, (int)ProcessModeEnum.Disabled);
            SetDeferred(RigidBody2D.PropertyName.Freeze, true);
            SetDeferred(RigidBody2D.PropertyName.ContactMonitor, false);

            if (_collisionShape != null)
            {
                _collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
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
                _pickupRadius = activeConfig.GetStat("Radius").CalculateFinal();
                velocityComponent.maxSpeed = activeConfig.GetStat("MoveSpeed").CalculateFinal();
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            if (!_isActive) return;

            if (!IsInstanceValid(_player))
            {
                _player = this.GetPlayer();
                if (_player == null) return;
            }

            float distSq = GlobalPosition.DistanceSquaredTo(_player.GlobalPosition);

            if (SawPlayer || distSq <= _pickupRadius * _pickupRadius)
            {
                SawPlayer = true;
                pathfindComponent.PathfindToPlayer();
            }

            if (distSq <= CollectRadius * CollectRadius)
            {
                OnPickup(_player);
                Despawn();
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

        private bool Merge()
        {
            float mergeRadiusSq = MergeRadius * MergeRadius;
            Type targetType = GetType();
            for (int i = _activeAbsorbables.Count - 1; i >= 0; i--)
            {
                Absorbable other = _activeAbsorbables[i];
                if (!IsInstanceValid(other))
                {
                    _activeAbsorbables.RemoveAt(i);
                    continue;
                }
                if (other == this || !other._isActive || other.SawPlayer || other.GetType() != targetType)
                {
                    continue;
                }
                if (GlobalPosition.DistanceSquaredTo(other.GlobalPosition) <= mergeRadiusSq)
                {
                    other.AddAmount(GetAmount());
                    return true;
                }
            }

            return false;
        }

        protected virtual void OnPickup(Player player)
        {
        }

        public virtual float GetAmount() => 0f;

        public virtual void SetAmount(float amount)
        {
        }

        public virtual void AddAmount(float amount) => SetAmount(GetAmount() + amount);
    }
}
