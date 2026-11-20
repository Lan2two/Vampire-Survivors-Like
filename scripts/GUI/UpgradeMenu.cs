namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;
    using System.Collections.Generic;

    public enum Rarity { Common, Rare, Epic, Legendary }

    public partial class UpgradeMenu : Control
    {
        private const int MaxUpgradeButtons = 5;

        [ExportGroup("Loot Table")]
        [Export] private Array<Strategy> CommonTable = new();
        [Export] private Array<Strategy> RareTable = new();
        [Export] private Array<Strategy> EpicTable = new();
        [Export] private Array<Strategy> LegendaryTable = new();

        [Export] private float CommonWeight = 60;
        [Export] private float RareWeight = 25;
        [Export] private float EpicWeight = 12;
        [Export] private float LegendaryWeight = 3;

        [ExportGroup("Settings")]
        [Export] public int Rerolls = 2;
        [Export(PropertyHint.Range, "0, 5, 1")] private int StartingButtons { get; set; } = 3;

        [ExportGroup("Scenes")]
        [Export] private XpBar xpBar;
        [Export] private PackedScene button;

        private List<CustomButton> _activeButtons = new();
        private List<UpgradeChoiceData> _activeUpgrades = new();

        private InventoryManager inventory;
        private HBoxContainer _upgradeButtonsContainer;
        private Button _rerollButton;
        private Button _skipButton;
        private int _rerollCount;

        private struct UpgradeChoiceData
        {
            public Rarity RarityType;
            public Strategy SelectedStrategy;
        }

        public override void _Ready()
        {
            var player = this.GetPlayer();
            if (player != null)
            {
                inventory = player.inventoryManager;
            }

            _upgradeButtonsContainer = GetNodeOrNull<HBoxContainer>("UpgradeButtonContainer");
            _rerollButton = GetNodeOrNull<Button>("UpgradeMenuContainer/RerollButton");
            _skipButton = GetNodeOrNull<Button>("UpgradeMenuContainer/SkipButton");

            if (xpBar == null || _upgradeButtonsContainer == null || _rerollButton == null || _skipButton == null)
            {
                GD.PrintErr("Missing critical node references in UpgradeMenu.");
                return;
            }

            _rerollButton.Pressed += RerollButton;
            _skipButton.Pressed += SkipButton;
            GameScene.GameOverEvent += Hide;
            VisibilityChanged += SetReroll;
            xpBar.LevelUpEvent += DisplayUpgrades;
            EventBus.AddButton += AddButton;

            SetReroll();
            for (int i = 0; i < StartingButtons; i++)
            {
                AddButton();
            }

            CallDeferred(MethodName.DisplayUpgrades);
        }

        public override void _ExitTree()
        {
            if (_rerollButton != null) _rerollButton.Pressed -= RerollButton;
            if (_skipButton != null) _skipButton.Pressed -= SkipButton;
            GameScene.GameOverEvent -= Hide;
            VisibilityChanged -= SetReroll;
            if (xpBar != null) xpBar.LevelUpEvent -= DisplayUpgrades;
            EventBus.AddButton -= AddButton;
            UnbindButtonEvents();
        }

        public void AddButton()
        {
            if (_activeButtons.Count >= MaxUpgradeButtons)
            {
                GD.Print("Max Buttons reached");
                return;
            }

            CustomButton buttonInstance = button.Instantiate<CustomButton>();
            if (buttonInstance == null)
            {
                GD.PrintErr("Button instance is not of type CustomButton");
                return;
            }

            buttonInstance.Visible = true;
            _upgradeButtonsContainer.AddChild(buttonInstance);
            _activeButtons.Add(buttonInstance);
            buttonInstance.Material = buttonInstance.Material?.Duplicate() as Material;

            buttonInstance.CustomButtonPressed += ButtonPressed;
            buttonInstance.CustomButtonHovered += OnHover;
            buttonInstance.MouseExited += ResetScale;
        }

        private void UnbindButtonEvents()
        {
            foreach (CustomButton btn in _activeButtons)
            {
                if (IsInstanceValid(btn))
                {
                    btn.CustomButtonPressed -= ButtonPressed;
                    btn.CustomButtonHovered -= OnHover;
                    btn.MouseExited -= ResetScale;
                }
            }
        }

        private void ButtonPressed(CustomButton btn)
        {
            int buttonIndex = _activeButtons.IndexOf(btn);
            if (buttonIndex >= 0 && buttonIndex < _activeUpgrades.Count)
            {
                AddUpgrade(buttonIndex);
                Visible = false;
                GetTree().Paused = false;
            }
            else
            {
                GD.PrintErr($"No upgrade is assigned to button index: {buttonIndex}");
            }
        }
        private void DisplayUpgrades()
        {
            RandUpgrade();
            Visible = true;
            GetTree().Paused = true;
        }
        public void RandUpgrade()
        {
            _activeUpgrades.Clear();
            float totalWeight = CommonWeight + RareWeight + EpicWeight + LegendaryWeight;
            HashSet<Strategy> pickedThisRoll = new();

            for (int i = 0; i < _activeButtons.Count; i++)
            {
                CustomButton btn = _activeButtons[i];
                Array<Strategy> table;
                Rarity rarity = Rarity.Common;
                Strategy randStrategy = null;

                int attempts = 0;
                const int maxAttempts = 10;

                while (randStrategy == null && attempts < maxAttempts)
                {
                    ++attempts;
                    float randtable = (float)GD.RandRange(0f, totalWeight);
                    if (randtable <= LegendaryWeight)
                    {
                        table = LegendaryTable;
                        rarity = Rarity.Legendary;
                        btn.SetShader(Colors.Black, Colors.Goldenrod, .1f, true);
                    }
                    else if (randtable <= LegendaryWeight + EpicWeight)
                    {
                        table = EpicTable;
                        rarity = Rarity.Epic;
                        btn.SetShader(Colors.Black, Colors.WebPurple, .1f, true);
                    }
                    else if (randtable <= LegendaryWeight + EpicWeight + RareWeight)
                    {
                        table = RareTable;
                        rarity = Rarity.Rare;
                        btn.SetShader(Colors.Black, Colors.NavyBlue, .1f, true);
                    }
                    else
                    {
                        table = CommonTable;
                        rarity = Rarity.Common;
                        btn.SetShader(Colors.Black, Colors.DimGray, .1f, true);
                    }

                    if (table == null || table.Count == 0)
                    {
                        table = CommonTable;
                        rarity = Rarity.Common;
                    }

                    if (table == null || table.Count == 0)
                        break;

                    int randIndex = GD.RandRange(0, table.Count - 1);
                    Strategy candidate = table[randIndex];

                    if (!pickedThisRoll.Contains(candidate))
                    {
                        randStrategy = candidate;
                        pickedThisRoll.Add(randStrategy);
                    }
                }

                if (randStrategy == null)
                {
                    table = CommonTable.Count > 0 ? CommonTable : RareTable.Count > 0 ? RareTable : EpicTable.Count > 0 ? EpicTable : LegendaryTable;
                    if (table != null && table.Count > 0)
                    {
                        randStrategy = table[GD.RandRange(0, table.Count - 1)];
                        rarity = table == CommonTable ? Rarity.Common : table == RareTable ? Rarity.Rare : table == EpicTable ? Rarity.Epic : Rarity.Legendary;
                    }
                }

                if (randStrategy == null)
                {
                    btn.Visible = false;
                    continue;
                }

                btn.Visible = true;

                UpgradeChoiceData choice = new UpgradeChoiceData
                {
                    RarityType = rarity,
                    SelectedStrategy = randStrategy
                };
                _activeUpgrades.Add(choice);

                btn.Icon = randStrategy.ItemIcon;
                btn.Text = $"{randStrategy.ItemName}\n{randStrategy.GetDescription()}\n{randStrategy.Count}/{randStrategy.MaxStackSize}";
            }
        }

        private void AddUpgrade(int i)
        {
            if (i < 0 || i >= _activeUpgrades.Count) return;

            Strategy chosenUpgrade = _activeUpgrades[i].SelectedStrategy;
            if (inventory != null)
            {
                inventory.AddItem(chosenUpgrade);
            }

            chosenUpgrade.Count++;

            if (chosenUpgrade.Count >= chosenUpgrade.MaxStackSize)
            {
                Array<Strategy> table = _activeUpgrades[i].RarityType switch
                {
                    Rarity.Common => CommonTable,
                    Rarity.Rare => RareTable,
                    Rarity.Epic => EpicTable,
                    Rarity.Legendary => LegendaryTable,
                    _ => CommonTable
                };
                table.Remove(chosenUpgrade);
                GD.Print($"{chosenUpgrade.ItemName} has been maxed out and removed from loot pool.");
            }
        }
        private void SetReroll()
        {
            _rerollCount = Rerolls;
            if (_rerollButton != null)
            {
                _rerollButton.Text = $"Reroll {_rerollCount}/{Rerolls}";
            }
        }
        private void RerollButton()
        {
            if (_rerollCount <= 0) return;
            RandUpgrade();
            _rerollCount--;
            _rerollButton.Text = $"Reroll {_rerollCount}/{Rerolls}";
        }
        private void SkipButton()
        {
            Visible = false;
            GetTree().Paused = false;
        }
        private void OnHover(CustomButton btn)
        {
            btn.AnimateButtonScale(new Vector2(1.1f, 1.1f));
            foreach (CustomButton child in _activeButtons)
            {
                if (child == btn) continue;
                child.AnimateButtonScale(new Vector2(0.95f, 0.95f));
            }
        }
        private void ResetScale()
        {
            foreach (CustomButton child in _activeButtons)
            {
                child.AnimateButtonScale(new Vector2(1f, 1f), 0.12f);
            }
        }
    }
}