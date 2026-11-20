namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class PlayerSpeedStrategy : Strategy, IStatUpgrade
    {
        [Export] float UpgradeSpeed { get; set; } = 10f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;
        public float Amount => UpgradeSpeed;

        public PlayerSpeedStrategy()
        {
            upgradeType = UpgradeType.Player;
            ItemName = "Agility";
            ItemDescription = "More speed";
        }

        public void ApplyStat(IUpgradable upgradable, float totalAmount)
        {
            Resource config = upgradable.currentConfig;
            if (config is PlayerStatsData stats)
            {
                stats.MoveSpeed = CalculateStatUpgrade(
                    stats.MoveSpeed,
                    totalAmount,
                    CalculationMode
                );
            }
        }
    }
}
