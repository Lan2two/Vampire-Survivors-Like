namespace SurvivorsGame
{
    using Godot;

    public interface IStatStrategy
    {
        void ApplyUpgrade(Resource config);
    }

    public interface IStatUpgrade
    {
        float Amount { get; }
        UpgradeCalculationMode CalculationMode { get; }
        void ApplyStat(Resource config, float totalAmount);
    }
    public interface IAttachment
    {
        void ApplyUpgrade(Node node);
    }
}
