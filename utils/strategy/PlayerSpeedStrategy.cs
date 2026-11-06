namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class PlayerSpeedStrategy : Strategy
    {
        [Export] float UpgradeSpeed { get; set; } = 10f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;


        public PlayerSpeedStrategy()
        {
            upgradeType = UpgradeType.Player;
            ItemName = "Agility";
            ItemDescription = "More speed";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is PlayerStatsData stats)
            {
                if (CalculationMode == UpgradeCalculationMode.Percentage)
                {
                    stats.MoveSpeedPercent += UpgradeSpeed;
                }
                else
                {
                    stats.MoveSpeedAdditive += UpgradeSpeed;
                }
            }
        }
    }
}
