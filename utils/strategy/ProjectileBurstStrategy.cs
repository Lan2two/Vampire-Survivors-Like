namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class ProjectileBurstStrategy : Strategy
    {
        [Export] int BurstAmount { get; set; } = 3;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;

        public ProjectileBurstStrategy()
        {
            upgradeType = UpgradeType.Gun;
            ItemName = "Burst shots";
            ItemDescription = "Bullet bursts";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is ProjectileData damageable)
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
