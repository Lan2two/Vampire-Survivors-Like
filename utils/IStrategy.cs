namespace SurvivorsGame
{
    using Godot;

    public interface IStrategy
    {
        void ApplyUpgrade(Resource config);
    }

    public interface IAttachment
    {
        void ApplyNode(Node node);
    }
    public interface IStatUpgrade
    {
        float Amount { get; }
        UpgradeCalculationMode CalculationMode { get; }
        void ApplyStat(Resource config, float totalAmount);
    }
}
