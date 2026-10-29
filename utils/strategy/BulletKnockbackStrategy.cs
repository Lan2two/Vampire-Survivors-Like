namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletKnockbackStrategy : Strategy, IStatUpgrade
    {
        [Export] float UpgradeKnockback { get; set; } = 10f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;
        public float Amount => UpgradeKnockback;

        public BulletKnockbackStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Heavier bullets";
            ItemDescription = "Bullet deals more knockback";
        }

        public void ApplyStat(IUpgradable upgradable, float totalAmount)
        {
            Resource config = upgradable.currentConfig;
            if (config is IDamageDealing damageable)
            {
                damageable.Knockback = CalculateStatUpgrade(
                    damageable.Knockback,
                    totalAmount,
                    CalculationMode);
            }
        }
    }
}
