using Godot;
using System;

public partial class UpgradeManager : Node
{
    [Export] InventoryManager inventoryManager;

    public void ApplyUpgrades(Resource config, UpgradeType targetType)
    {
        foreach (Strategy strategy in inventoryManager.Upgrades)
        {
            if (strategy.upgradeType == targetType && strategy is IStrategy s)
            {
                s.ApplyUpgrade(config);
            }
        }
    }
}
