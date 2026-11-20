namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletFireRateStrategy : Strategy
    {
        [Export] float UpgradeCooldown { get; set; } = 15f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;

        public BulletFireRateStrategy()
        {
            upgradeType = UpgradeType.Projectile;
            ItemName = "Faster Shooting";
            ItemDescription = "The gun fires more often";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is ProjectileData bulletData)
            {
                if (CalculationMode == UpgradeCalculationMode.Percentage)
                {
                    bulletData.ProjectileCooldownPercent -= UpgradeCooldown;
                }
                else
                {
                    bulletData.ProjectileCooldownAdditive -= UpgradeCooldown;
                }
            }
        }
    }
}
