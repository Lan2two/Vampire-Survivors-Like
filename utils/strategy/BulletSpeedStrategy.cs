namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletSpeedStrategy : Strategy, IStrategy, IStatUpgrade
    {
        [Export] float UpgradeSpeed { get; set; } = 25f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;
        public float Amount => UpgradeSpeed;

        public BulletSpeedStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Bullet Speed";
            ItemDescription = "Bullet travels faster";
        }
        public void ApplyUpgrade(Resource config)
        {
            ApplyUpgrade(config, UpgradeSpeed);
        }

        public void ApplyUpgrade(Resource config, float totalAmount)
        {
            if (config is BulletData damageable)
            {
                damageable.Speed = CalculateStatUpgrade(
                    damageable.Speed,
                    totalAmount,
                    CalculationMode);
            }
        }
    }
}
