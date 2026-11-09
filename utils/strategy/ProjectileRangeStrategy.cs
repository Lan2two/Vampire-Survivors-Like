namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class ProjectileRangeStrategy : Strategy
    {
        [Export] float UpgradeRange { get; set; } = 25f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;

        public ProjectileRangeStrategy()
        {
            upgradeType = UpgradeType.Gun;
            ItemName = "Sharpshooter";
            ItemDescription = "Bullet travels farther";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is ProjectileData bulletData)
            {
                if (CalculationMode == UpgradeCalculationMode.Percentage)
                {
                    bulletData.MaxTravelDistancePercent += UpgradeRange;
                }
                else
                {
                    bulletData.MaxTravelDistanceAdditive += UpgradeRange;
                }
            }
        }
    }
}
