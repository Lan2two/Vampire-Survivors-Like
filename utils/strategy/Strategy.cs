namespace SurvivorsGame
{
    using Godot;

    public enum UpgradeType
    {
        Bullet,
        Melee
    }

    public interface IStatUpgrade
    {
        float Amount { get; }
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

        public virtual string GetDescription()
        {
            return ItemDescription;
        }
    }
}
