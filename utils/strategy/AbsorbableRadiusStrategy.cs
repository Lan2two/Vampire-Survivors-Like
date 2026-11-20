namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class AbsorbableRadiusStrategy : Strategy
    {
        [Export] float UpgradeRadius { get; set; } = 10f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;

        public AbsorbableRadiusStrategy()
        {
            upgradeType = UpgradeType.Absorbable;
            ItemName = "Absorbable Range";
            ItemDescription = "Increases range of pickup";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is AbsorbableData data)
            {
                if (CalculationMode == UpgradeCalculationMode.Percentage)
                {
                    data.RadiusPercent += UpgradeRadius;
                }
                else
                {
                    data.RadiusAdditive += UpgradeRadius;
                }
            }
        }
    }
}
