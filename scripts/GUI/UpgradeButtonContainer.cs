namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;
    public partial class UpgradeButtonContainer : HBoxContainer
    {
        private const int MaxUpgradeButtons = 5;
        [Export] public Array<Strategy> Strategies = new();
        [Export] private XpBar xpBar;
        [Export(PropertyHint.Range, "0,5,1")] private int StartingButtons { get; set; } = 3;
        [Export] Button button;
        private Dictionary<string, Button> _UpgradeButtons = new();
        private Array<Strategy> _RandUpgrades = new();
        private InventoryManager inventory;
        private Control _parent;
        private int count;

        public override void _Ready()
        {
            _parent = GetParent() as Control;
            inventory = this.GetPlayer().inventoryManager;
            if (xpBar == null)
            {
                GD.PrintErr("XpBar unassigned in UpgradeButtonContainer");
                return;
            }
            if (Strategies.Count == 0)
            {
                GD.PrintErr("No upgrades assigned in UpgradesMenu");
                return;
            }
            if (inventory == null)
            {
                GD.Print("Inventory from player not found in UpgradeButtonContainer");
            }
            for (int i = 0; i < StartingButtons; i++)
            {
                AddButton();
            }
            DisplayUpgrades();
            xpBar.LevelUpEvent += DisplayUpgrades;
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

        public void AddButton()
        {
            if (count >= MaxUpgradeButtons)
            {
                GD.Print("Max Buttons");
                return;
            }
            count++;
            Button buttonInstance = button.Duplicate() as Button;
            buttonInstance.Visible = true;
            buttonInstance.Name = $"{buttonInstance.Name}{count}";
            AddChild(buttonInstance);
            foreach (Node child in GetChildren())
            {
                if (child is UpgradeButton button)
                {
                    string buttonName = button.Name.ToString();
                    if (_UpgradeButtons.ContainsKey(buttonName))
                    {
                        continue;
                    }
                    _UpgradeButtons.Add(buttonName, button);
                    button.UpgradeButtonPressed += ButtonPressed;
                }
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
                _parent.Visible = false;
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
            _parent.Visible = true;
            GetTree().Paused = true;
        }
        public void RandUpgrade()
        {
            _RandUpgrades.Clear();
            foreach (UpgradeButton button in _UpgradeButtons.Values)
            {
                int randIndex = GD.RandRange(0, Strategies.Count - 1);
                Strategy randStrategy = Strategies[randIndex];

                _RandUpgrades.Add(randStrategy);
                button.Icon = randStrategy.ItemIcon;
                button.Text = $"{randStrategy.ItemName}\n{randStrategy.GetDescription()}\n{randStrategy.Count}/{randStrategy.MaxStackSize}";

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
            //GD.Print(_RandUpgrades[i].ItemName);
            chosenUpgrade.Count++;
            if (chosenUpgrade.Count >= chosenUpgrade.MaxStackSize)
            {
                Strategies.Remove(chosenUpgrade);
                GD.Print($"{chosenUpgrade.ItemName} has been removed from upgrade pool");
            }

        }
    }
}