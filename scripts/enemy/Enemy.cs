namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class Enemy : RigidBody2D, IPoolable
    {
        [Export] float DespawnRadius { get; set; } = 420f;
        public float XpAmount = 1f;
        public float CoinAmount = 5f;
        public AnimatedSprite2D animatedSprite;
        public CollisionShape2D collisionbox;
        public HealthComponent healthComponent;
        public VelocityComponent velocityComponent;
        public EnemyDeath enemyDeath;
        public StateMachine stateMachine;
        public KnockbackComponent knockbackComponent;
        public StatusManager statusManager;
        public static int Alive = 0;

        public bool IsActive { get; private set; }
        public Action<Enemy> ReturnToPool { get; set; }

        private uint _defaultLayer;
        private uint _defaultMask;
        private float _baseRadius;

        private double timer;

        public override void _Ready()
        {
            _defaultLayer = CollisionLayer;
            _defaultMask = CollisionMask;

            animatedSprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
            collisionbox = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
            healthComponent = GetNodeOrNull<HealthComponent>("HealthComponent");
            velocityComponent = GetNodeOrNull<VelocityComponent>("VelocityComponent");
            stateMachine = GetNodeOrNull<StateMachine>("StateMachine");
            enemyDeath = GetNodeOrNull<EnemyDeath>("StateMachine/Death");
            knockbackComponent = GetNodeOrNull<KnockbackComponent>("KnockbackComponent");
            statusManager = GetNodeOrNull<StatusManager>("StatusManager");

            if (collisionbox?.Shape is CircleShape2D circle)
            {
                collisionbox.Shape = (Shape2D)circle.Duplicate();
                _baseRadius = circle.Radius;
            }

            if (healthComponent != null)
            {
                healthComponent.FinalDamageEvent += TrackDamage;
            }
        }

        public override void _ExitTree()
        {
            if (healthComponent != null) healthComponent.FinalDamageEvent -= TrackDamage;
            if (IsActive) Alive--;
        }

        public void OnSpawn() => SetActive(true);
        public void OnDespawn() => SetActive(false);

        public void Despawn()
        {
            if (IsActive) ReturnToPool?.Invoke(this);
        }

        public override void _PhysicsProcess(double delta)
        {
            timer -= delta;
            if (timer > 0) return;
            timer = 1;
            if (GlobalPosition.DistanceSquaredTo(this.GetPlayer().GlobalPosition) > DespawnRadius * DespawnRadius)
            {
                //GD.Print($"Despawned from distance, {GlobalPosition.DistanceSquaredTo(this.GetPlayer().GlobalPosition)}");
                Despawn();
            }
        }


        private void SetActive(bool active)
        {
            if (IsActive != active)
            {
                IsActive = active;
                Alive += active ? 1 : -1;
            }

            Visible = active;
            SetDeferred(Node.PropertyName.ProcessMode, (int)(active ? ProcessModeEnum.Inherit : ProcessModeEnum.Disabled));
            SetDeferred(RigidBody2D.PropertyName.Freeze, !active);

            if (active)
            {
                AddToGroup("enemies");
                LinearVelocity = Vector2.Zero;
                AngularVelocity = 0f;
                if (_defaultLayer != 0) CollisionLayer = _defaultLayer;
                if (_defaultMask != 0) CollisionMask = _defaultMask;
                enemyDeath?.Reset();
                stateMachine?.Reset();
                knockbackComponent?.Reset();
            }
            else
            {
                RemoveFromGroup("enemies");
            }

            ToggleColliders(this, active);
        }

        private static void ToggleColliders(Node root, bool active)
        {
            foreach (Node child in root.GetChildren())
            {
                if (child is CollisionShape2D shape) shape.SetDeferred(CollisionShape2D.PropertyName.Disabled, !active);
                else if (child is Area2D area)
                {
                    area.SetDeferred(Area2D.PropertyName.Monitoring, active);
                    area.SetDeferred(Area2D.PropertyName.Monitorable, active);
                }
                if (child.GetChildCount() > 0) ToggleColliders(child, active);
            }
        }

        public void SetStats(float hp = 15f, float defense = 0, float maxSpeed = 25f, float coinAmount = 2f, float xpAmount = 2f, float scale = 1f, Color color = default)
        {
            if (healthComponent == null || velocityComponent == null || animatedSprite == null) return;

            float safeScale = scale <= 0f ? 1f : scale;
            healthComponent.maxHealth = hp;
            healthComponent.currentHealth = hp;
            healthComponent.Defense = defense;
            velocityComponent.maxSpeed = maxSpeed;
            Modulate = color == default ? Colors.White : color;
            CoinAmount = coinAmount;
            XpAmount = xpAmount;

            foreach (Node child in GetChildren())
            {
                if (child is Node2D node2D)
                {
                    node2D.Scale = new Vector2(safeScale, safeScale);
                }
            }
        }


        private void TrackDamage(Attack attack) => Stats.data.DamageDealt += attack.Damage;
    }
}
