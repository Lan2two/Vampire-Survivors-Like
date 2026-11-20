namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletSpreadStrategy : Strategy
    {
        [Export] float AddSpread { get; set; } = .3f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;

        public BulletSpreadStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Inaccuracy";
            ItemDescription = "Bullet Spread";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is BulletData bulletData)
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
