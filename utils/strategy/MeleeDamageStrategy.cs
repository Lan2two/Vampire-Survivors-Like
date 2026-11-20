namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class MeleeDamageStrategy : Strategy
    {
        [Export] float UpgradeDamage { get; set; } = .2f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;

        public MeleeDamageStrategy()
        {
            upgradeType = UpgradeType.Melee;
            ItemName = "Biceps";
            ItemDescription = "Melee deals more damage";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is MeleeWeaponBase meleeWeapon)
            {
                if (CalculationMode == UpgradeCalculationMode.Percentage)
                {
                    meleeWeapon.DamagePercent += UpgradeDamage;
                }
                else
                {
                    meleeWeapon.DamageBase += UpgradeDamage;
                }
            }
        }
    }
}
