namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class MeleeDamageStrategy : Strategy, IStatUpgrade
    {
        [Export] float UpgradeDamage { get; set; } = 5f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;
        public float Amount => UpgradeDamage;

        public MeleeDamageStrategy()
        {
            upgradeType = UpgradeType.Melee;
            ItemName = "Biceps";
            ItemDescription = "Melee deals more damage";
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
