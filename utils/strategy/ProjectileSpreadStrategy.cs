namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class ProjectileSpreadStrategy : Strategy
    {
        [Export] float AddSpread { get; set; } = .3f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;

        public ProjectileSpreadStrategy()
        {
            upgradeType = UpgradeType.Gun;
            ItemName = "Inaccuracy";
            ItemDescription = "Bullet Spread";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is ProjectileData bulletData)
            {
                if (CalculationMode == UpgradeCalculationMode.Percentage)
                {
                    bulletData.SpreadPercent += AddSpread;
                }
                else
                {
                    bulletData.SpreadAdditive += AddSpread;
                }
            }
        }
    }
}
