namespace SurvivorsGame
{
    using System;
    using Godot;

    public partial class EventBus
    {
        public static event Action AddButton;
        public static event Action PlayerUpgrade;
        public static event Action MeleeUpgrade;
        public static event Action GunUpgrade;
        public static event Action KnivesUpgrade;
        public static event Action AbsorbableUpgrade;

        public static void InvokeAddButton()
        {
            AddButton?.Invoke();
            GD.Print("Add buttons invoked");
        }

        public static void InvokeUpgrades(Strategy strategy)
        {
            if (strategy == null)
            {
                return;
            }
            UpgradeType type = strategy.upgradeType;
            switch (type)
            {
                case UpgradeType.Player:
                    PlayerUpgrade?.Invoke();
                    GD.Print("Player upgrades invoked");
                    break;
                case UpgradeType.Melee:
                    MeleeUpgrade?.Invoke();
                    GD.Print("Melee upgrades invoked");
                    break;
                case UpgradeType.Gun:
                    GunUpgrade?.Invoke();
                    GD.Print("Gun upgrades invoked");
                    break;
                case UpgradeType.Knives:
                    KnivesUpgrade?.Invoke();
                    GD.Print("Knives upgrades invoked");
                    break;
                case UpgradeType.Absorbable:
                    AbsorbableUpgrade?.Invoke();
                    GD.Print("Absorbable upgrades invoked");
                    break;
                default:
                    break;
            }
        }
    }
}