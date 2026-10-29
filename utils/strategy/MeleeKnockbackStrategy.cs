namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class MeleeKnockbackStrategy : Strategy, IStatUpgrade
    {
        [Export] float UpgradeKnockback { get; set; } = 10f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;
        public float Amount => UpgradeKnockback;

        public MeleeKnockbackStrategy()
        {
            upgradeType = UpgradeType.Melee;
            ItemName = "Melee Knockback";
            ItemDescription = "Melee deals more knockback";
        }

        public void ApplyStat(IUpgradable upgradable, float totalAmount)
        {
            Resource config = upgradable.currentConfig;
            if (config is IDamageDealing damageable)
            {
                damageable.Knockback = CalculateStatUpgrade(
                    damageable.Knockback,
                    totalAmount,
                    CalculationMode);
            }
        }
    }
}
