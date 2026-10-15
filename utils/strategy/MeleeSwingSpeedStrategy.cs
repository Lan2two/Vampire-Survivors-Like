namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class MeleeSwingSpeedStrategy : Strategy, IStrategy
    {
        [Export] float UpgradeSwingSpeed { get; set; } = 1f;

        public MeleeSwingSpeedStrategy()
        {
            upgradeType = UpgradeType.Melee;
            ItemName = "Melee Swing Speed";
            ItemDescription = "Melee swings faster";
        }
        public void ApplyUpgrade(Resource config)
        {
            if (config is MeleeWeaponBase swingSpeedMultiplier)
            {
                swingSpeedMultiplier.SwingSpeedMultiplier += UpgradeSwingSpeed;
            }
        }
    }
}
