namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class PlayerHealthStrategy : Strategy
    {
        [Export] float UpgradeHealth { get; set; } = 10f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;

        public PlayerHealthStrategy()
        {
            upgradeType = UpgradeType.Player;
            ItemName = "Big Boi";
            ItemDescription = "More Hp";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is PlayerStatsData stats)
            {
                if (CalculationMode == UpgradeCalculationMode.Percentage)
                {
                    stats.MaxHealthPercent += UpgradeHealth;
                }
                else
                {
                    stats.MaxHealthAdditive += UpgradeHealth;
                }
            }
        }
    }
}
