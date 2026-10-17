namespace SurvivorsGame
{
    using Godot;

    public enum UpgradeType
    {
        Bullet,
        Melee
    }
    [GlobalClass]
    public partial class Strategy : ItemData
    {
        public Strategy()
        {
            ResourceLocalToScene = true;
        }
        [Export] public int UpgradePool { get; set; } = 3;
        public UpgradeType upgradeType;
    }
}
