namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class MeleeKnockbackStrategy : Strategy, IStatStrategy, IStatUpgrade
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
        public void ApplyUpgrade(Resource config)
        {
            ApplyStat(config, UpgradeKnockback);
        }

        public void ApplyStat(Resource config, float totalAmount)
        {
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
