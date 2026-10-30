namespace SurvivorsGame
{
    using Godot;

    public enum UpgradeType
    {
        Combo,
        Bullet,
        Melee,
        Player
    }

    public enum UpgradeCalculationMode
    {
        Additive,
        Percentage
    }

    [GlobalClass]
    public partial class Strategy : ItemData
    {
        public Strategy()
        {
            ResourceLocalToScene = true;
            MaxStackSize = 5;
        }
        public int Count { get; set; }
        public virtual UpgradeType upgradeType { get; set; }

        protected static float CalculateStatUpgrade(
            float currentValue,
            float amount,
            UpgradeCalculationMode calculationMode)
        {
            return calculationMode == UpgradeCalculationMode.Percentage
                ? currentValue * (1f + amount / 100f)
                : currentValue + amount;
        }

        public virtual string GetDescription()
        {
            return ItemDescription;
        }

        public virtual void ApplyUpgrade(IUpgradable upgradable)
        { }
    }
}
