namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletPierceStrategy : Strategy
    {
        [Export] int UpgradePierce { get; set; } = 1;

        public BulletPierceStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Piercing Rounds";
            ItemDescription = "Bullet pierces";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is BulletData damageable)
            {
                damageable.PierceCount += UpgradePierce;
            }
        }
    }
}
