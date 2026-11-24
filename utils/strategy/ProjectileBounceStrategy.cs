namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class ProjectileBounceStrategy : Strategy
    {
        [Export] int BounceCount { get; set; } = 1;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;

        public ProjectileBounceStrategy()
        {
            upgradeType = UpgradeType.Gun;
            ItemName = "Bouncy Rounds";
            ItemDescription = "Adds +1 to the amount of times bullets may bounce";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is ProjectileData damageable)
            {
                if (CalculationMode == UpgradeCalculationMode.Percentage)
                {
                    damageable.BouncePercent += BounceCount;
                }
                else
                {
                    damageable.BounceAdditive += BounceCount;
                }
            }
        }
    }
}
