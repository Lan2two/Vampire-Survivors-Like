namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class ProjectileCritRateStrategy : Strategy
    {
        [Export] float UpgradeCritRate { get; set; } = 5;

        public ProjectileCritRateStrategy()
        {
            upgradeType = UpgradeType.Gun;
            ItemName = "Luck of The Draw";
            ItemDescription = "Increase Bullet Crit Rate";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is ProjectileData damageable)
            {
                damageable.CritRateAdditive += UpgradeCritRate;
            }
        }
    }
}
