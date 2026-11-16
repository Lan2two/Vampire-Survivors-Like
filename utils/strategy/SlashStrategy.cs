namespace SurvivorsGame
{
    using Godot;
    [GlobalClass]
    public partial class SlashStrategy : Strategy, IEventUpgrade
    {
        public SlashStrategy()
        {
            ItemName = "World Cutting Slash";
            ItemDescription = "Splits the world into two and kills all enemies on screen";
            MaxStackSize = 2;
        }
        public void ApplyUpgrade()
        {
            EventBus.InvokeSlash();
        }
    }
}