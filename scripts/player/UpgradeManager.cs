namespace SurvivorsGame
{
    using Godot;

    public partial class UpgradeManager : Node
    {
        private const int CompositeDepth = 10;
        [Export] InventoryManager inventoryManager;

        public void ApplyUpgrades(IUpgradable upgradable, UpgradeType targetType)
        {
            foreach (Strategy strategy in inventoryManager.Upgrades)
            {
                ApplyStrategy(strategy, upgradable, targetType, 0);
            }
        }

        public static void InvokeUpgrades(Strategy strategy)
        {
            if (strategy is IEventUpgrade eventUpgrade)
            {
                eventUpgrade.ApplyUpgrade();
            }
            if (strategy is CompositeStrategy composite)
            {
                foreach (Strategy child in composite.Children)
                {
                    EventBus.InvokeUpgrades(child);
                }
            }
            else
            {
                EventBus.InvokeUpgrades(strategy);
            }
        }

        private void ApplyStrategy(
            Strategy strategy,
            IUpgradable upgradable,
            UpgradeType targetType,
            int depth)
        {
            if (strategy == null)
            {
                return;
            }
            if (strategy is CompositeStrategy composite)
            {
                ApplyComposite(composite, upgradable, targetType, depth);
                return;
            }
            if (strategy.upgradeType != targetType)
            {
                return;
            }
            else
            {
                strategy.ApplyUpgrade(upgradable);
            }
        }

        private void ApplyComposite(
            CompositeStrategy composite,
            IUpgradable upgradable,
            UpgradeType targetType,
            int depth)
        {
            if (depth >= CompositeDepth)
            {
                GD.PrintErr($"{composite.ItemName} is nested too deeply");
                return;
            }

            foreach (Strategy child in composite.Children)
            {
                ApplyStrategy(child, upgradable, targetType, depth + 1);
            }
        }
    }
}
