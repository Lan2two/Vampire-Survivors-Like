namespace SurvivorsGame
{
    using Godot;
    [GlobalClass]
    public partial class MoreUpgradeStrategy : Strategy
    {
        public MoreUpgradeStrategy()
        {
            upgradeType = UpgradeType.Event;
            ItemName = "More Buttons";
            ItemDescription = "Adds another upgrade button";
            MaxStackSize = 2;
        }
        public override void ApplyUpgrade()
        {
            EventBus.InvokeAddButton();
        }
    }
}