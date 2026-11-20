namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletKnockbackStrategy : Strategy, IStrategy
    {
        [Export] float UpgradeKnockback { get; set; } = 10f;
        public BulletKnockbackStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Bullet Knockback";
            ItemDescription = "Bullet deals more knockback";
        }
        public void ApplyUpgrade(Resource config)
        {
            if (config is IDamageDealing damageable)
            {
                damageable.Knockback += UpgradeKnockback;
            }
        }
    }
}
