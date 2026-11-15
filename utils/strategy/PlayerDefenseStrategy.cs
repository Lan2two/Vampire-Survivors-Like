namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class PlayerDefenseStrategy : Strategy
    {
        [Export] float UpgradeDefense { get; set; } = 10f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;

        public PlayerDefenseStrategy()
        {
            upgradeType = UpgradeType.Player;
            ItemName = "Iron Skin";
            ItemDescription = "More Defense";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is PlayerStatsData stats)
            {
                if (CalculationMode == UpgradeCalculationMode.Percentage)
                {
                    stats.DefensePercent += UpgradeDefense;
                }
                else
                {
                    stats.DefenseAdditive += UpgradeDefense;
                }
            }
        }
    }
}
