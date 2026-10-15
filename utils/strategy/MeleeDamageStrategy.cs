namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class MeleeDamageStrategy : Strategy, IStrategy
    {
        [Export] float UpgradeDamage { get; set; } = 5f;
        public MeleeDamageStrategy()
        {
            upgradeType = UpgradeType.Melee;
            ItemName = "Melee Damage";
            ItemDescription = "Melee deals more damage";
        }
        public void ApplyUpgrade(Resource config)
        {
            if (config is IDamageDealing damageable)
            {
                damageable.Damage += UpgradeDamage;
            }
        }
    }
}
