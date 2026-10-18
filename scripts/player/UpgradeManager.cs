namespace SurvivorsGame
{
    using Godot;

    public partial class UpgradeManager : Node
    {
        private const int CompositeDepth = 4;
        [Export] InventoryManager inventoryManager;

        public void ApplyUpgrades(Resource config, UpgradeType targetType)
        {
            foreach (Strategy strategy in inventoryManager.Upgrades)
            {
                ApplyStrategy(strategy, config, targetType, 0);
            }
        }

        private void ApplyStrategy(Strategy strategy, Resource config, UpgradeType targetType, int depth)
        {
            if (strategy == null)
            {
                return;
            }
            if (strategy is CompositeStrategy composite)
            {
                if (depth >= CompositeDepth)
                {
                    GD.PrintErr($"{composite.ItemName} is nested too deeply");
                    return;
                }
                foreach (Strategy child in composite.Children)
                {
                    ApplyStrategy(child, config, targetType, depth + 1);
                }
                return;
            }
            if (strategy.upgradeType == targetType && strategy is IStrategy upgrade)
            {
                upgrade.ApplyUpgrade(config);
            }
        }
    }
}
