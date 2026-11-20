namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class MeleeKnockbackStrategy : Strategy, IStrategy, IStatUpgrade
    {
        [Export] float UpgradeKnockback { get; set; } = 10f;
        public float Amount => UpgradeKnockback;

        public MeleeKnockbackStrategy()
        {
            upgradeType = UpgradeType.Melee;
            ItemName = "Melee Knockback";
            ItemDescription = "Melee deals more knockback";
        }
        public void ApplyUpgrade(Resource config)
        {
            ApplyUpgrade(config, UpgradeKnockback);
        }

        public void ApplyUpgrade(Resource config, float totalAmount)
        {
            if (config is IDamageDealing damageable)
            {
                damageable.Knockback += totalAmount;
            }
        }
    }
}
