namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class MeleeCritDamageStrategy : Strategy
    {
        [Export] float UpgradeCritDamage { get; set; } = .2f;

        public MeleeCritDamageStrategy()
        {
            upgradeType = UpgradeType.Melee;
            ItemName = "Melee Crit Damage";
            ItemDescription = "Increase Melee Crit Damage";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is MeleeWeaponBase damageable)
            {
                damageable.CritDamageMultiplier += UpgradeCritDamage;
            }
        }
    }
}
