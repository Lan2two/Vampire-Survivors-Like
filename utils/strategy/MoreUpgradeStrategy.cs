namespace SurvivorsGame
{
    using Godot;
    [GlobalClass]
    public partial class MoreUpgradeStrategy : Strategy, IEventUpgrade
    {
        public MoreUpgradeStrategy()
        {
            upgradeType = UpgradeType.Event;
            ItemName = "More Buttons";
            ItemDescription = "Adds another upgrade button";
            MaxStackSize = 2;
        }
        public void ApplyUpgrade()
        {
            EventBus.InvokeAddButton();
        }
    }
}