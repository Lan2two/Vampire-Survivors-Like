namespace SurvivorsGame
{
    using Godot;
    using System.Collections.Generic;

    public partial class UpgradeManager : Node
    {
        private const int CompositeDepth = 4;
        [Export] InventoryManager inventoryManager;

        public void ApplyUpgrades(Resource config, UpgradeType targetType)
        {
            List<StatUpgradeTotal> statUpgrades = new();
            foreach (Strategy strategy in inventoryManager.Upgrades)
            {
                ApplyStrategy(strategy, config, targetType, 0, statUpgrades);
            }

            foreach (StatUpgradeTotal total in statUpgrades)
            {
                total.Upgrade.ApplyUpgrade(config, total.Amount);
            }
        }

        private void ApplyStrategy(
            Strategy strategy,
            Resource config,
            UpgradeType targetType,
            int depth,
            List<StatUpgradeTotal> statUpgrades)
        {
            if (strategy == null)
            {
                return;
            }
            if (strategy is CompositeStrategy composite)
            {
                ApplyComposite(composite, config, targetType, depth, statUpgrades);
                return;
            }
            if (strategy.upgradeType != targetType)
            {
                return;
            }

            if (strategy is IStatUpgrade statUpgrade)
            {
                AccumulateStatUpgrade(statUpgrade, statUpgrades);
            }
            else if (strategy is IStrategy upgrade)
            {
                upgrade.ApplyUpgrade(config);
            }
        }

        private void ApplyComposite(
            CompositeStrategy composite,
            Resource config,
            UpgradeType targetType,
            int depth,
            List<StatUpgradeTotal> statUpgrades)
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
        }

        private static void AccumulateStatUpgrade(
            IStatUpgrade upgrade,
            List<StatUpgradeTotal> statUpgrades)
        {
            foreach (StatUpgradeTotal total in statUpgrades)
            {
                if (total.Upgrade.GetType() == upgrade.GetType()
                    && total.Upgrade.CalculationMode == upgrade.CalculationMode)
                {
                    total.Amount += upgrade.Amount;
                    return;
                }
            }

            statUpgrades.Add(new StatUpgradeTotal(upgrade, upgrade.Amount));
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
