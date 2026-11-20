namespace SurvivorsGame
{
    using Godot;

    public enum UpgradeType
    {
        Bullet,
        Melee,
        Player
    }

    public enum UpgradeCalculationMode
    {
        Additive,
        Percentage
    }

    public interface IStatUpgrade
    {
        float Amount { get; }
        UpgradeCalculationMode CalculationMode { get; }
        void ApplyUpgrade(Resource config, float totalAmount);
    }

    [GlobalClass]
    public partial class Strategy : ItemData
    {
        public Strategy()
        {
            ResourceLocalToScene = true;
        }
        [Export]
        public int UpgradePool { get; set; } = 3;
        public int Count { get; set; }
        public UpgradeType upgradeType;

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
    }
}
