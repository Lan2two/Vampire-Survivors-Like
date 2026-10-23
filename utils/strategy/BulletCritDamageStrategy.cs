namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletCritDamageStrategy : Strategy, IStrategy
    {
        [Export] float UpgradeCritDamage { get; set; } = 5;

        public BulletCritDamageStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Bullet Crit Damage";
            ItemDescription = "Increase Bullet Crit Damage";
        }

        public void ApplyUpgrade(Resource config)
        {
            if (config is BulletData damageable)
            {
                damageable.CritDamageMultiplier += UpgradeCritDamage;
            }
        }
    }
}
