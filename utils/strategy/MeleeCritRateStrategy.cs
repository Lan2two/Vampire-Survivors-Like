namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class MeleeCritRateStrategy : Strategy, IStrategy
    {
        [Export] float UpgradeCritRate { get; set; } = 5;

        public MeleeCritRateStrategy()
        {
            upgradeType = UpgradeType.Melee;
            ItemName = "Melee Crit Rate";
            ItemDescription = "Increase Melee Crit Rate";
        }

        public void ApplyUpgrade(Resource config)
        {
            if (config is MeleeWeaponBase damageable)
            {
                damageable.CritRate += UpgradeCritRate;
            }
        }
    }
}
