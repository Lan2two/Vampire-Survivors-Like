namespace SurvivorsGame
{
    using Godot;
    [GlobalClass]
    public partial class InflictStatusStrategy : Strategy, IEventUpgrade
    {
        [Export] StatusEffect status;
        public InflictStatusStrategy()
        {
            ItemName = "Global Infliction";
            ItemDescription = $"Applies status to all enemies on screen";
            MaxStackSize = 2;
        }
        public void ApplyUpgrade()
        {
            EventBus.InvokeInflictStatus(status);
        }
    }
}