namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletCritRateStrategy : Strategy, IStrategy
    {
        [Export] float UpgradeCritRate { get; set; } = .2f;

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
