namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class ProjectileDetectionRangeStrategy : Strategy
    {
        [Export] float UpgradeDetectionRange { get; set; } = 5f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Percentage;

        public ProjectileDetectionRangeStrategy()
        {
            upgradeType = UpgradeType.Gun;
            ItemName = "Long Sight";
            ItemDescription = "Increases range of detection for this weapon";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is ProjectileData data)
            {
                if (CalculationMode == UpgradeCalculationMode.Percentage)
                {
                    data.DetectionRangePercent += UpgradeDetectionRange;
                }
                else
                {
                    data.DetectionRangeAdditive += UpgradeDetectionRange;
                }
            }
        }
    }
}
