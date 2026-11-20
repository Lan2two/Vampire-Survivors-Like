namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletCritRateStrategy : Strategy, IStrategy
    {
        [Export] int UpgradeCritRate { get; set; } = 5;

        public BulletCritRateStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Bullet Crit Rate";
            ItemDescription = "Increase Bullet Crit Rate";
        }

        public void ApplyUpgrade(Resource config)
        {
            if (config is BulletData damageable)
            {
                damageable.CritRate += UpgradeCritRate;
            }
        }
    }
}
