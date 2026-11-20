namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class ProjectileBurstIntervalStrategy : Strategy
    {
        [Export] float BurstTime { get; set; } = 0.05f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;

        public ProjectileBurstIntervalStrategy()
        {
            upgradeType = UpgradeType.Gun;
            ItemName = "Burst Interval";
            ItemDescription = "Affects time between shots in bursts";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is ProjectileData damageable)
            {
                if (CalculationMode == UpgradeCalculationMode.Percentage)
                {
                    damageable.BurstIntervalPercent -= BurstTime;
                }
                else
                {
                    damageable.BurstIntervalAdditive -= BurstTime;
                }
            }
        }
    }
}
