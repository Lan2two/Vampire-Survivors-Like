namespace SurvivorsGame
{
    using System;
    using Godot;

    public partial class EventBus
    {
        public static event Action AddButton;
        public static event Action PlayerUpgrade;
        public static event Action MeleeUpgrade;
        public static event Action BulletUpgrade;

        public static void InvokeAddButton()
        {
            AddButton?.Invoke();
            GD.Print("Add buttons invoked");
        }

        public static void InvokeUpgrades(Strategy strategy)
        {
            if (strategy.upgradeType == UpgradeType.Player)
            {
                PlayerUpgrade?.Invoke();
                GD.Print("Player upgrades invoked");
            }
            if (strategy.upgradeType == UpgradeType.Melee)
            {
                MeleeUpgrade?.Invoke();
                GD.Print("Melee upgrades invoked");
            }
            if (strategy.upgradeType == UpgradeType.Bullet)
            {
                BulletUpgrade?.Invoke();
                GD.Print("Bullet upgrades invoked");
            }
        }
    }
}