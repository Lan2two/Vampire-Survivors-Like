namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class PlayerSpeedStrategy : Strategy, IStrategy, IStatUpgrade
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
        public void ApplyUpgrade(Resource config)
        {
            ApplyStat(config, UpgradeSpeed);
        }

        public void ApplyStat(Resource config, float totalAmount)
        {
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
