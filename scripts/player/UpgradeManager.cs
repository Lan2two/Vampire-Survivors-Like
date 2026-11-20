using Godot;
using System;

public partial class UpgradeManager : Node
{
    [Export] InventoryManager inventoryManager;

    public void UpgradeBullet(Bullet spawnedBullet)
    {
        foreach (Strategy strategy in inventoryManager.Upgrades)
        {
            if (strategy.upgradeType == UpgradeType.Bullet && strategy is IStrategy bulletStrategy)
            {
                bulletStrategy.ApplyUpgrade(spawnedBullet);
            }
        }
    }

    public void UpgradeMelee(MeleeWeapon meleeWeapon)
    {
        foreach (Strategy strategy in inventoryManager.Upgrades)
        {
            if (strategy.upgradeType == UpgradeType.Melee && strategy is IStrategy meleeStrategy)
            {
                meleeStrategy.ApplyUpgrade(meleeWeapon);
            }
        }
    }
}
