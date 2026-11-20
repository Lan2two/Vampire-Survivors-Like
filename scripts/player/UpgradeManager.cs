namespace SurvivorsGame
{
    using Godot;
    using System.Collections.Generic;
    using System;

    public partial class UpgradeManager : Node
    {
        private const int CompositeDepth = 4;
        [Export] InventoryManager inventoryManager;

        public void ApplyUpgrades(Resource config, UpgradeType targetType)
        {
            Dictionary<Type, StatUpgradeTotal> statUpgrades = new();
            foreach (Strategy strategy in inventoryManager.Upgrades)
            {
                ApplyStrategy(strategy, config, targetType, 0, statUpgrades);
            }

            foreach (StatUpgradeTotal total in statUpgrades.Values)
            {
                total.Upgrade.ApplyUpgrade(config, total.Amount);
            }
        }

        private void ApplyStrategy(
            Strategy strategy,
            Resource config,
            UpgradeType targetType,
            int depth,
            Dictionary<Type, StatUpgradeTotal> statUpgrades)
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
                    ApplyStrategy(child, config, targetType, depth + 1, statUpgrades);
                }
                return;
            }
            if (strategy.upgradeType != targetType)
            {
                return;
            }

            if (strategy is IStatUpgrade statUpgrade)
            {
                Type upgradeType = statUpgrade.GetType();
                if (statUpgrades.TryGetValue(upgradeType, out StatUpgradeTotal existing))
                {
                    existing.Amount += statUpgrade.Amount;
                }
                else
                {
                    statUpgrades.Add(
                        upgradeType,
                        new StatUpgradeTotal(statUpgrade, statUpgrade.Amount));
                }
            }
            else if (strategy is IStrategy upgrade)
            {
                upgrade.ApplyUpgrade(config);
            }
        }

        private sealed class StatUpgradeTotal
        {
            public StatUpgradeTotal(IStatUpgrade upgrade, float amount)
            {
                Upgrade = upgrade;
                Amount = amount;
            }

            public IStatUpgrade Upgrade { get; }
            public float Amount { get; set; }
        }
    }
}
