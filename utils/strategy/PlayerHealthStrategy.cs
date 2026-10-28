namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class PlayerHealthStrategy : Strategy, IStatStrategy, IStatUpgrade
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
        public void ApplyUpgrade(Resource config)
        {
            ApplyStat(config, UpgradeHealth);
        }

        public void ApplyStat(Resource config, float totalAmount)
        {
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
