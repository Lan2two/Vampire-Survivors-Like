namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletSpeedStrategy : Strategy, IStatUpgrade
    {
        [Export] float UpgradeSpeed { get; set; } = 25f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;
        public float Amount => UpgradeSpeed;

        public BulletSpeedStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Fast Shot";
            ItemDescription = "Bullet travels faster";
        }

        public void ApplyStat(IUpgradable upgradable, float totalAmount)
        {
            Resource config = upgradable.currentConfig;
            if (config is BulletData damageable)
            {
                damageable.Speed = CalculateStatUpgrade(
                    damageable.Speed,
                    totalAmount,
                    CalculationMode);
            }
        }
    }
}
