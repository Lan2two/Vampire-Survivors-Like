namespace SurvivorsGame
{
    using Godot;
    [GlobalClass]
    public partial class RerollStrategy : Strategy, IEventUpgrade
    {
        public RerollStrategy()
        {
            ItemName = "Rerolls";
            ItemDescription = "Adds rerolls to upgrades";
            MaxStackSize = 5;
        }
        public void ApplyUpgrade()
        {
            EventBus.InvokeAddReroll();
        }
    }
}