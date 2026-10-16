namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;
    using System;
    public partial class UpgradeMenu : HBoxContainer
    {
        [Export] public Array<Strategy> Strategies = new();
        [Export] private XpBar xpBar;
        private Dictionary<string, Button> _UpgradeButtons = new();
        private Array<Strategy> _RandUpgrades = new();
        private InventoryManager inventory;

        public override void _Ready()
        {
            foreach (Node child in GetChildren())
            {
                if (child is UpgradeButton button)
                {
                    string buttonName = button.Name.ToString();
                    _UpgradeButtons.Add(buttonName, button);
                    button.UpgradeButtonPressed += ButtonPressed;
                }
            }
            if (xpBar == null)
            {
                GD.PrintErr("XpBar unassigned in UpgradeMenu");
                return;
            }
            if (Strategies.Count == 0)
            {
                GD.PrintErr("No upgrades assigned in UpgradesMenu");
                return;
            }
            inventory = this.GetPlayer().inventoryManager;
            xpBar.LevelUpEvent += DisplayUpgrades;
            RandUpgrade();
        }

        public override void _ExitTree()
        {
            xpBar.LevelUpEvent -= DisplayUpgrades;
            foreach (UpgradeButton button in _UpgradeButtons.Values)
            {
                button.UpgradeButtonPressed -= ButtonPressed;
            }
        }

        private void ButtonPressed(UpgradeButton button)
        {
            string buttonName = button.Name.ToString();
            var keysList = new System.Collections.Generic.List<string>(_UpgradeButtons.Keys);
            int buttonIndex = keysList.IndexOf(buttonName);

            if (buttonIndex >= 0 && buttonIndex < _RandUpgrades.Count)
            {
                AddUpgrade(buttonIndex);
                Visible = false;
                GetTree().Paused = false;
            }
            else
            {
                GD.PrintErr($"No upgrade is assigned to button '{buttonName}'");
            }
        }
        private void DisplayUpgrades()
        {
            RandUpgrade();
            Visible = true;
            GetTree().Paused = true;
        }
        private void RandUpgrade()
        {
            _RandUpgrades.Clear();
            foreach (UpgradeButton button in _UpgradeButtons.Values)
            {
                int randIndex = GD.RandRange(0, Strategies.Count - 1);
                Strategy randStrategy = Strategies[randIndex];
                randStrategy.UpgradePool--;
                _RandUpgrades.Add(randStrategy);
                button.Icon = randStrategy.ItemIcon;
                button.Text = $"{randStrategy.ItemName}\n{randStrategy.ItemDescription}";
                if (randStrategy.UpgradePool <= 0)
                {
                    Strategies.Remove(randStrategy);
                }
            }

        }
        private void AddUpgrade(int i)
        {
            if (i < 0 || i >= _RandUpgrades.Count)
            {
                GD.PrintErr($"Upgrade index {i} is out of range");
                return;
            }

            inventory.AddItem(_RandUpgrades[i]);
            GD.Print(_RandUpgrades[i].ItemName);
        }
    }
}