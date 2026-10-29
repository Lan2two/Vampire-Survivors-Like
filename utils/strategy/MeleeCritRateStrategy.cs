namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class MeleeCritRateStrategy : Strategy
    {
        [Export] float UpgradeCritRate { get; set; } = 5;

        public MeleeCritRateStrategy()
        {
            upgradeType = UpgradeType.Melee;
            ItemName = "Melee Crit Rate";
            ItemDescription = "Increase Melee Crit Rate";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is MeleeWeaponBase damageable)
            {
                damageable.CritRate += UpgradeCritRate;
            }
        }
    }
}
