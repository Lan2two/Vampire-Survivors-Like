namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class MeleeSwingSpeedStrategy : Strategy, IStatStrategy, IStatUpgrade
    {
        [Export] float UpgradeSwingSpeed { get; set; } = 1f;
        [Export] public UpgradeCalculationMode CalculationMode { get; set; } = UpgradeCalculationMode.Additive;
        public float Amount => UpgradeSwingSpeed;

        public MeleeSwingSpeedStrategy()
        {
            upgradeType = UpgradeType.Melee;
            ItemName = "Fast Attacks";
            ItemDescription = "Melee swings faster";
        }
        public void ApplyUpgrade(Resource config)
        {
            ApplyStat(config, UpgradeSwingSpeed);
        }

        public void ApplyStat(Resource config, float totalAmount)
        {
            if (config is MeleeWeaponBase swingSpeedMultiplier)
            {
                swingSpeedMultiplier.SwingSpeedMultiplier = CalculateStatUpgrade(
                    swingSpeedMultiplier.SwingSpeedMultiplier,
                    totalAmount,
                    CalculationMode);
            }
        }
    }
}
