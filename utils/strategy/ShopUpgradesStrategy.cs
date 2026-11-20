namespace SurvivorsGame
{
    using Godot;
    [GlobalClass]
    public partial class ShopUpgradesStrategy : Strategy
    {
        public ShopUpgradesStrategy()
        {
            upgradeType = UpgradeType.Shop;
            ItemName = "More Buttons";
            ItemDescription = "Adds another choice upon upgrading";
            MaxStackSize = 2;
        }
        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            if (upgradable is UpgradeMenu menu)
            {
                Count++;
                menu.AddButton();
            }
        }
    }
}