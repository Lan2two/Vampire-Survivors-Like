namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class MeleeDamageStrategy : Strategy, IStrategy, IStatUpgrade
    {
        [Export] float UpgradeDamage { get; set; } = 5f;
        public float Amount => UpgradeDamage;

        public MeleeDamageStrategy()
        {
            upgradeType = UpgradeType.Melee;
            ItemName = "Melee Damage";
            ItemDescription = "Melee deals more damage";
        }
        public void ApplyUpgrade(Resource config)
        {
            ApplyUpgrade(config, UpgradeDamage);
        }

        public void ApplyUpgrade(Resource config, float totalAmount)
        {
            if (config is IDamageDealing damageable)
            {
                damageable.Damage += totalAmount;
            }
        }
    }
}
