namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;
    using System;

    public partial class UpgradeMenu : Control
    {
        private const int MaxUpgradeButtons = 5;
        [Export] public int Rerolls = 2;
        [Export] public Array<Strategy> Strategies = new();
        [Export] private XpBar xpBar;
        [Export(PropertyHint.Range, "0,5,1")] private int StartingButtons { get; set; } = 3;
        [Export] PackedScene button;
        private Dictionary<string, Button> _UpgradeButtons = new();
        private Array<Strategy> _RandUpgrades = new();
        private InventoryManager inventory;
        private int count;
        private HBoxContainer _upgradeButtonsContainer;
        private Button _rerollButton;
        private Button _skipButton;
        private int _rerollCount;

        public override void _Ready()
        {
            inventory = this.GetPlayer().inventoryManager;
            _upgradeButtonsContainer = GetNodeOrNull<HBoxContainer>("UpgradeButtonContainer");
            _rerollButton = GetNodeOrNull<Button>("UpgradeMenuContainer/RerollButton");
            _skipButton = GetNodeOrNull<Button>("UpgradeMenuContainer/SkipButton");
            if (xpBar == null)
            {
                GD.PrintErr("XpBar unassigned in HBoxContainer");
                return;
            }
            if (Strategies.Count == 0)
            {
                GD.PrintErr("No upgrades assigned in UpgradesMenu");
                return;
            }
            if (inventory == null)
            {
                GD.Print("Inventory from player not found in GridContainer");
            }
            if (_upgradeButtonsContainer == null)
            {
                GD.PrintErr("HBoxContainer not found in UpgradeMenu");
                return;
            }
            if (_rerollButton == null)
            {
                GD.PrintErr("RerollButton not found in UpgradeMenu");
                return;
            }
            if (_skipButton == null)
            {
                GD.PrintErr("RerollButton not found in UpgradeMenu");
                return;
            }
            for (int i = 0; i < StartingButtons; i++)
            {
                AddButton();
            }
            _rerollButton.Pressed += RerollButton;
            _skipButton.Pressed += SkipButton;
            GameScene.GameOverEvent += Hide;
            VisibilityChanged += SetReroll;
            xpBar.LevelUpEvent += DisplayUpgrades;
            EventBus.AddButton += AddButton;
            SetReroll();
            DisplayUpgrades();

        }

        public override void _ExitTree()
        {
            if (_rerollButton != null)
            {
                _rerollButton.Pressed -= RerollButton;
            }
            if (_skipButton != null)
            {
                _skipButton.Pressed -= SkipButton;
            }
            GameScene.GameOverEvent -= Hide;
            VisibilityChanged -= SetReroll;
            if (xpBar != null)
            {
                xpBar.LevelUpEvent -= DisplayUpgrades;
            }
            EventBus.AddButton -= AddButton;
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
            Button buttonInstance = button.Instantiate<Button>();
            buttonInstance.Visible = true;
            buttonInstance.Name = $"{buttonInstance.Name}{count}";
            _upgradeButtonsContainer.AddChild(buttonInstance);
            foreach (Node child in _upgradeButtonsContainer.GetChildren())
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
            GD.Print(_RandUpgrades[i].ItemName);
            chosenUpgrade.Count++;
            if (chosenUpgrade.Count >= chosenUpgrade.MaxStackSize)
            {
                Strategies.Remove(chosenUpgrade);
                GD.Print($"{chosenUpgrade.ItemName} has been removed from upgrade pool");
            }

        }

        private void SetReroll()
        {
            _rerollCount = Rerolls;
            _rerollButton.Text = $"Reroll {_rerollCount}/{Rerolls}";
        }

        private void RerollButton()
        {
            if (_rerollCount <= 0)
            {
                return;
            }
            RandUpgrade();
            _rerollCount--;
            _rerollButton.Text = $"Reroll {_rerollCount}/{Rerolls}";

        }

        private void SkipButton()
        {
            Visible = false;
            GetTree().Paused = false;
        }

    }
}