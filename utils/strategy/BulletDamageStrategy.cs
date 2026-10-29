namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletDamageStrategy : Strategy, IStatUpgrade
    {
        [Export] float UpgradeDamage { get; set; } = 5f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;
        public float Amount => UpgradeDamage;

        public BulletDamageStrategy()
        {
            upgradeType = UpgradeType.Bullet;
            ItemName = "Better bullets";
            ItemDescription = "Bullet deals more damage";
        }

        public void ApplyStat(IUpgradable upgradable, float totalAmount)
        {
            Resource config = upgradable.currentConfig;
            if (config is IDamageDealing damageable)
            {
                damageable.Damage = CalculateStatUpgrade(
                    damageable.Damage,
                    totalAmount,
                    CalculationMode);
            }
        }
    }
}
