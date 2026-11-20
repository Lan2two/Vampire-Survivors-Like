namespace SurvivorsGame
{
    public interface IStatUpgrade
    {
        float Amount { get; }
        UpgradeCalculationMode CalculationMode { get; }
        void ApplyStat(IUpgradable upgradable, float totalAmount);
    }
    public interface IEventUpgrade
    {
        void ApplyUpgrade();
    }
}
