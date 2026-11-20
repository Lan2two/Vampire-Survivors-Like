namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletCritRateStrategy : Strategy
    {
        [Export] float UpgradeCritRate { get; set; } = 5;

        public BulletCritRateStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Luck of The Draw";
            ItemDescription = "Increase Bullet Crit Rate";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is BulletData damageable)
            {
                damageable.CritRate += UpgradeCritRate;
            }
        }
    }
}
