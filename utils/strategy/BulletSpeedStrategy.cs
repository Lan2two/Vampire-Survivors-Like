namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletSpeedStrategy : Strategy
    {
        [Export] float UpgradeSpeed { get; set; } = 25f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;

        public BulletSpeedStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Fast Shot";
            ItemDescription = "Bullet travels faster";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is ProjectileData bulletData)
            {
                if (CalculationMode == UpgradeCalculationMode.Percentage)
                {
                    bulletData.SpeedPercent += UpgradeSpeed;
                }
                else
                {
                    bulletData.SpeedAdditive += UpgradeSpeed;
                }
            }
        }
    }
}
