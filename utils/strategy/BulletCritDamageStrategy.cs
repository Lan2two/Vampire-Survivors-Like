namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletCritDamageStrategy : Strategy
    {
        [Export] float UpgradeCritDamage { get; set; } = 5;

        public BulletCritDamageStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Crit Damage";
            ItemDescription = "Increase Bullet Crit Damage";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is BulletData damageable)
            {
                damageable.CritDamageMultiplier += UpgradeCritDamage;
            }

        }
    }
}
