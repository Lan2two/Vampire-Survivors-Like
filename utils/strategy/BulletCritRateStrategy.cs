namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletCritRateStrategy : Strategy, IStatStrategy
    {
        [Export] float UpgradeCritRate { get; set; } = .2f;

        public BulletCritRateStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Luck of The Draw";
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
