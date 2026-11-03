namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class MeleeSwingSpeedStrategy : Strategy
    {
        [Export] float UpgradeSwingSpeed { get; set; } = 1f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;

        public MeleeSwingSpeedStrategy()
        {
            upgradeType = UpgradeType.Melee;
            ItemName = "Fast Attacks";
            ItemDescription = "Melee swings faster";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is MeleeWeaponBase meleeWeapon)
            {
                if (CalculationMode == UpgradeCalculationMode.Percentage)
                {
                    meleeWeapon.SwingCooldownPercent -= UpgradeSwingSpeed;
                }
                else
                {
                    meleeWeapon.SwingCooldownAdditive -= UpgradeSwingSpeed;
                }
            }
        }
    }
}
