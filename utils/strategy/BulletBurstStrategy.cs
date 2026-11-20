namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletBurstStrategy : Strategy
    {
        [Export] int BurstAmount { get; set; } = 3;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;

        public BulletBurstStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Burst shots";
            ItemDescription = "Bullet bursts";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is BulletData damageable)
            {
                if (CalculationMode == UpgradeCalculationMode.Percentage)
                {
                    damageable.BurstPercent += BurstAmount;
                }
                else
                {
                    damageable.BurstAdditive += BurstAmount;
                }
            }
        }
    }
}
