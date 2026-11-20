namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class MeleeKnockbackStrategy : Strategy
    {
        [Export] float UpgradeKnockback { get; set; } = 10f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;

        public MeleeKnockbackStrategy()
        {
            upgradeType = UpgradeType.Melee;
            ItemName = "Melee Knockback";
            ItemDescription = "Melee deals more knockback";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is MeleeWeaponBase meleeWeapon)
            {
                if (CalculationMode == UpgradeCalculationMode.Percentage)
                {
                    meleeWeapon.KnockbackPercent += UpgradeKnockback;
                }
                else
                {
                    meleeWeapon.KnockbackAdditive += UpgradeKnockback;
                }
            }
        }
    }
}
