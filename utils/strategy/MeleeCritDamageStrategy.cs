namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class MeleeCritDamageStrategy : Strategy, IStrategy
    {
        [Export] float UpgradeCritDamage { get; set; } = .2f;

        public MeleeCritDamageStrategy()
        {
            upgradeType = UpgradeType.Melee;
            ItemName = "Melee Crit Damage";
            ItemDescription = "Increase Melee Crit Damage";
        }

        public void ApplyUpgrade(Resource config)
        {
            if (config is MeleeWeaponBase damageable)
            {
                damageable.CritDamageMultiplier += UpgradeCritDamage;
            }
        }
    }
}
