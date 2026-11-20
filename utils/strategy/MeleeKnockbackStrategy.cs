namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class MeleeKnockbackStrategy : Strategy, IStrategy
    {
        [Export] float UpgradeKnockback { get; set; } = 10f;
        public MeleeKnockbackStrategy()
        {
            upgradeType = UpgradeType.Melee;
            ItemName = "Melee Knockback";
            ItemDescription = "Melee deals more knockback";
        }
        public void ApplyUpgrade(Resource config)
        {
            if (config is IDamageDealing damageable)
            {
                damageable.Knockback += UpgradeKnockback;
            }
        }
    }
}
