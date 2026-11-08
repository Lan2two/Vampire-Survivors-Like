namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletBurstIntervalStrategy : Strategy
    {
        [Export] float BurstTime { get; set; } = 0.05f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;

        public BulletBurstIntervalStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Burst Interval";
            ItemDescription = "Affects time between shots in bursts";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is BulletData damageable)
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
