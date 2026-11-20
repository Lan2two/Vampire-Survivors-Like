namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletBounceStrategy : Strategy
    {
        public BulletBounceStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Bullet Bounce";
            ItemDescription = "Bullet bounces between enemies";
            MaxStackSize = 1;
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is BulletData damageable)
            {
                damageable.BouncingBullets = true;
            }

        }
    }
}
