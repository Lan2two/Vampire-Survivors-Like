namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class PlayerHealthStrategy : Strategy, IStatUpgrade
    {
        [Export] float UpgradeHealth { get; set; } = 10f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;
        public float Amount => UpgradeHealth;

        public PlayerHealthStrategy()
        {
            upgradeType = UpgradeType.Player;
            ItemName = "Big Boi";
            ItemDescription = "More Hp";
        }

        public void ApplyStat(IUpgradable upgradable, float totalAmount)
        {
            Resource config = upgradable.currentConfig;
            if (config is PlayerStatsData stats)
            {
                stats.MaxHealth = CalculateStatUpgrade(
                    stats.MaxHealth,
                    totalAmount,
                    CalculationMode
                );
            }
        }
    }
}
