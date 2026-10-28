namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletFireRateStrategy : Strategy, IStrategy, IStatUpgrade
    {
        [Export] float UpgradeCooldown { get; set; } = 0.15f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;
        public float Amount => UpgradeCooldown;

        public BulletFireRateStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Faster Shooting";
            ItemDescription = "The gun fires more often";
        }

        public void ApplyUpgrade(Resource config)
        {
            ApplyStat(config, UpgradeCooldown);
        }

        public void ApplyStat(Resource config, float totalAmount)
        {
            if (config is BulletData bulletData)
            {
                if (CalculationMode == UpgradeCalculationMode.Percentage)
                {
                    float rateMultiplier = Mathf.Max(0.01f, 1f + totalAmount / 100f);
                    bulletData.GunCooldown = Mathf.Max(
                        0.05f,
                        bulletData.GunCooldown / rateMultiplier);
                }
                else
                {
                    bulletData.GunCooldown = Mathf.Max(
                        0.05f,
                        bulletData.GunCooldown - totalAmount);
                }
            }
        }
    }
}
