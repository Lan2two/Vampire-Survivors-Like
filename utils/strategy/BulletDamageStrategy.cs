namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletDamageStrategy : Strategy
    {
        [Export] float UpgradeDamage { get; set; } = 5f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;

        public BulletDamageStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Better bullets";
            ItemDescription = "Bullet deals more damage";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is BulletData damageable)
            {
                if (CalculationMode == UpgradeCalculationMode.Percentage)
                {
                    damageable.DamagePercent += UpgradeDamage;
                }
                else
                {
                    damageable.DamageAdditive += UpgradeDamage;
                }
            }
        }
    }
}
