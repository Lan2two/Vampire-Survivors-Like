namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletKnockbackStrategy : Strategy, IStrategy, IStatUpgrade
    {
        [Export] float UpgradeKnockback { get; set; } = 10f;
        public float Amount => UpgradeKnockback;

        public BulletKnockbackStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Bullet Knockback";
            ItemDescription = "Bullet deals more knockback";
        }
        public void ApplyUpgrade(Resource config)
        {
            ApplyUpgrade(config, UpgradeKnockback);
        }

        public void ApplyUpgrade(Resource config, float totalAmount)
        {
            if (config is IDamageDealing damageable)
            {
                damageable.Knockback += totalAmount;
            }
        }
    }
}
