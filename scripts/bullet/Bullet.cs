namespace SurvivorsGame
{
    using Godot;

    public partial class Bullet : Area2D, IWeapon
    {
        [Export] public BulletData config;
        private float traveledDistance = 0;
        private int hitCount = 0;
        private bool Crit;
        private float FinalDamage;
        public override void _Ready()
        {
            AreaEntered += OnAreaEntered;
        }

        public void Use(UpgradeManager upgradeManager)
        {
            config = config != null ? (BulletData)config.Duplicate() : new BulletData();
            upgradeManager.ApplyUpgrades(config, UpgradeType.Bullet);
            int Randint = GD.RandRange(0, 100);
            Crit = false;
            FinalDamage = config.Damage;
            if (Randint < config.CritRate)
            {
                Crit = true;
                FinalDamage *= config.CritDamageMultiplier;
            }
        }

        public override void _ExitTree()
        {
            AreaEntered -= OnAreaEntered;
        }

        public override void _PhysicsProcess(double delta)
        {
            Vector2 direction = Vector2.Left.Rotated(Rotation).Normalized();
            Position += direction * config.Speed * (float)delta;
            traveledDistance += config.Speed * (float)delta;

            if (config.MaxTravelDistance < traveledDistance || hitCount > config.PierceCount)
            {
                QueueFree();
            }
        }

        private void OnAreaEntered(Area2D area)
        {
            if (area is HurtboxComponent hurtbox)
            {
                if (hitCount <= config.PierceCount)
                {
                    Attack attack = new Attack
                    {
                        Damage = FinalDamage,
                        GlobalPosition = GlobalPosition,
                        KnockbackForce = config.Knockback,
                        Crit = Crit,
                    };
                    hurtbox.TakeDamage(attack);
                    hitCount += 1;
                }
            }
        }
    }
}
