namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;
    public partial class UpgradeMenu : HBoxContainer
    {
        [Export] public Array<Strategy> Strategies = new();
        [Export] private XpBar xpBar;
        private Dictionary<string, Button> _UpgradeButtons = new();
        private Array<Strategy> _RandUpgrades = new();
        private InventoryManager inventory;

        public override void _Ready()
        {
            inventory = this.GetPlayer().inventoryManager;
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
            if (inventory == null)
            {
                GD.Print("Inventory from player not found in UpgradeMenu");
            }
            xpBar.LevelUpEvent += DisplayUpgrades;
            DisplayUpgrades();
        }

        public override void _ExitTree()
        {
            if (xpBar != null)
            {
                xpBar.LevelUpEvent -= DisplayUpgrades;
            }
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
            if (Strategies is [])
            {
                GD.Print("No more upgrades");
                return;
            }
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

                _RandUpgrades.Add(randStrategy);
                button.Icon = randStrategy.ItemIcon;
                button.Text = $"{randStrategy.ItemName}\n{randStrategy.ItemDescription}";

            }

        }
        private void AddUpgrade(int i)
        {
            if (i < 0 || i >= _RandUpgrades.Count)
            {
                GD.PrintErr($"Upgrade index {i} is out of range");
                return;
            }
            Strategy chosenUpgrade = _RandUpgrades[i];
            inventory.AddItem(chosenUpgrade);
            GD.Print(_RandUpgrades[i].ItemName);
            chosenUpgrade.UpgradePool--;
            if (chosenUpgrade.UpgradePool <= 0)
            {
                Strategies.Remove(chosenUpgrade);
                GD.Print($"{chosenUpgrade.ItemName} has been removed from upgrade pool");
            }

        }
    }
}