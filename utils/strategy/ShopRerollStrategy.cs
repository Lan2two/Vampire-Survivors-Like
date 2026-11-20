namespace SurvivorsGame
{
    using Godot;
    [GlobalClass]
    public partial class ShopRerollStrategy : Strategy
    {
        public ShopRerollStrategy()
        {
            upgradeType = UpgradeType.Shop;
            ItemName = "Rerolls";
            ItemDescription = "Adds +1 reroll";
        }
        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            if (upgradable is UpgradeMenu menu)
            {
                Count++;
                menu.Rerolls = Count;
            }
        }
    }
}