namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class MeleeSwingSpeedStrategy : Strategy, IStrategy, IStatUpgrade
    {
        [Export] float UpgradeSwingSpeed { get; set; } = 1f;
        public float Amount => UpgradeSwingSpeed;

        public MeleeSwingSpeedStrategy()
        {
            upgradeType = UpgradeType.Melee;
            ItemName = "Melee Swing Speed";
            ItemDescription = "Melee swings faster";
        }
        public void ApplyUpgrade(Resource config)
        {
            ApplyUpgrade(config, UpgradeSwingSpeed);
        }

        public void ApplyUpgrade(Resource config, float totalAmount)
        {
            if (config is MeleeWeaponBase swingSpeedMultiplier)
            {
                swingSpeedMultiplier.SwingSpeedMultiplier += totalAmount;
            }
        }
    }
}
