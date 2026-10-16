namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletFireRateStrategy : Strategy, IStrategy
    {
        [Export] float UpgradeCooldown { get; set; } = 0.15f;

        public BulletFireRateStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Faster Shooting";
            ItemDescription = "The gun fires more often";
        }

        public void ApplyUpgrade(Resource config)
        {
            if (config is BulletData bulletData)
            {
                bulletData.GunCooldown = Mathf.Max(0.05f, bulletData.GunCooldown - UpgradeCooldown);
            }
        }
    }
}
