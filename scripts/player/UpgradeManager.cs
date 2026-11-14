namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    public partial class UpgradeManager : Node
    {
        private const int CompositeDepth = 10;
        [Export] InventoryManager inventoryManager;
        private Array<Strategy> ShopStrategies = new();
        private Array<Strategy> Strategies => [.. inventoryManager.Upgrades, .. ShopStrategies];
        public override void _Ready()
        {
            CallDeferred(MethodName.CallUpgrades);
        }

        public void ApplyUpgrades(IUpgradable upgradable, UpgradeType targetType)
        {
            foreach (Strategy strategy in Strategies)
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

        private void CallUpgrades()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, int> pair in SaveSystem.LoadUpgrades().PurchasedUpgrades)
            {
                if (pair.Value == 0) continue;
                for (int i = 0; i < pair.Value; i++)
                {
                    Strategy strategy = ResourceLoader.Load<Strategy>(pair.Key);
                    if (strategy == null)
                    {
                        GD.PrintErr($"Failed to load saved upgrade: {pair.Key}");
                        continue;
                    }
                    ShopStrategies.Add(strategy);
                    InvokeUpgrades(strategy);
                }
            }
        }
    }
}
