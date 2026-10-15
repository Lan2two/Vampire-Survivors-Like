namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletSpeedStrategy : Strategy, IStrategy
    {
        [Export] float UpgradeSpeed { get; set; } = 20f;
        public BulletSpeedStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Bullet Speed";
            ItemDescription = "Bullet travels faster";
        }
        public void ApplyUpgrade(Resource config)
        {
            if (config is BulletData damageable)
            {
                damageable.Speed += UpgradeSpeed;
            }
        }
    }
}
