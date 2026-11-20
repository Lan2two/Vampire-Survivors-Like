namespace SurvivorsGame
{
    using Godot;
    [GlobalClass]
    public partial class MoratoriumStrategy : Strategy, IEventUpgrade
    {
        public MoratoriumStrategy()
        {
            ItemName = "Moratorium";
            ItemDescription = "Applies the Moratorium status to all enemies on screen";
            MaxStackSize = 2;
        }
        public void ApplyUpgrade()
        {
            EventBus.InvokeMoratorium();
        }
    }
}