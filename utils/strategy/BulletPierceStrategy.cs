namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletPierceStrategy : Strategy, IStatStrategy
    {
        [Export] int UpgradePierce { get; set; } = 1;

        public BulletPierceStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Piercing Rounds";
            ItemDescription = "Bullet pierces";
        }

        public void ApplyUpgrade(Resource config)
        {
            if (config is BulletData damageable)
            {
                damageable.PierceCount += UpgradePierce;
            }
        }
    }
}
