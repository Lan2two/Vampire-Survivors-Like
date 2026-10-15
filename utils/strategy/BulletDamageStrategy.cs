namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletDamageStrategy : Strategy, IStrategy
    {
        [Export] float UpgradeDamage { get; set; } = 5f;
        public BulletDamageStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Bullet Damage";
            ItemDescription = "Bullet deals more damage";
        }
        public void ApplyUpgrade(Resource config)
        {
            if (config is IDamageDealing damageable)
            {
                damageable.Damage += UpgradeDamage;
            }
        }
    }
}
