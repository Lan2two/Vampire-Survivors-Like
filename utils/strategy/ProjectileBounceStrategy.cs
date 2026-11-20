namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class ProjectileBounceStrategy : Strategy
    {
        public ProjectileBounceStrategy()
        {
            upgradeType = UpgradeType.Gun;
            ItemName = "Bullet Bounce";
            ItemDescription = "Bullet bounces between enemies";
            MaxStackSize = 1;
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is ProjectileData damageable)
            {
                damageable.BouncingBullets = true;
            }

        }
    }
}
