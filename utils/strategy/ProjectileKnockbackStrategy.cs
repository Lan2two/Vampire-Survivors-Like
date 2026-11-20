namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class ProjectileKnockbackStrategy : Strategy
    {
        [Export] float UpgradeKnockback { get; set; } = 10f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;

        public ProjectileKnockbackStrategy()
        {
            upgradeType = UpgradeType.Gun;
            ItemName = "Heavier bullets";
            ItemDescription = "Bullet deals more knockback";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is ProjectileData bulletData)
            {
                if (CalculationMode == UpgradeCalculationMode.Percentage)
                {
                    bulletData.KnockbackPercent += UpgradeKnockback;
                }
                else
                {
                    bulletData.KnockbackAdditive += UpgradeKnockback;
                }
            }
        }
    }
}
