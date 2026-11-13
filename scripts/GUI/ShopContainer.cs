namespace SurvivorsGame
{
    using Godot;
    public partial class ShopContainer : SubMenuContainer
    {
        private GridContainer _gridContainer;
        private Label _costLabel;
        private Label _nameLabel;
        private RichTextLabel _desciptionLabel;
        private TextureRect _textureRect;
        private Panel _itemDisplay;
        private Button _upgradeButton;
        private Button _refundButton;
        private ShopItem _currentItem;
        private ChecksContainer _checksContainer;
        private GameSaveData gameSaveData;
        private UpgradeSaveData upgradeSaveData;
        public override void _Ready()
        {
            base._Ready();
            _gridContainer = GetNodeOrNull<GridContainer>("%GridContainer");
            _costLabel = GetNodeOrNull<Label>("%Label");
            _nameLabel = GetNodeOrNull<Label>("%ItemName");
            _desciptionLabel = GetNodeOrNull<RichTextLabel>("%ItemDescriptionLabel");
            _textureRect = GetNodeOrNull<TextureRect>("%TextureRect");
            _itemDisplay = GetNodeOrNull<Panel>("%ItemDisplay");
            _upgradeButton = GetNodeOrNull<Button>("%UpgradeButton");
            _refundButton = GetNodeOrNull<Button>("%RefundButton");
            _checksContainer = GetNodeOrNull<ChecksContainer>("%ChecksContainer");

            upgradeSaveData = SaveSystem.LoadUpgrades();

            foreach (ShopItem item in _gridContainer.GetChildren())
            {
                item.ShowDetail += ShowDetail;
            }
            _upgradeButton.Pressed += OnUpgrade;
            _refundButton.Pressed += OnRefund;
        }

        private void ShowDetail(ShopItem item)
        {
            _currentItem = item;
            _itemDisplay.Visible = true;
            _costLabel.Text = $"{item.cost}";
            _textureRect.Texture = item.textureRect.Texture;
            _desciptionLabel.Text = item.strategy.GetDescription();
            _nameLabel.Text = item.strategy.ItemName;
            _checksContainer.AddChecks(item.count);
            for (int i = 0; i < upgradeSaveData.PurchasedUpgrades[item.strategy.ResourcePath]; i++)
            {
                _checksContainer.ToggleBox(i);
            }
        }

        private void OnUpgrade()
        {
            if (_currentItem == null || _currentItem.strategy == null)
            {
                GD.PrintErr("Upgrade failed: Current item or strategy is null.");
                return;
            }

            gameSaveData = SaveSystem.LoadData();

            if (gameSaveData.TotalCoins < _currentItem.cost)
            {
                GD.Print("Not enough coins");
                return;
            }

            upgradeSaveData = SaveSystem.LoadUpgrades();
            string upgradeKey = _currentItem.strategy.ResourcePath;

            upgradeSaveData.PurchasedUpgrades.TryGetValue(upgradeKey, out int currentCount);

            if (currentCount >= _currentItem.count)
            {
                GD.Print("Max Stacks");
                return;
            }

            int newCount = currentCount + 1;
            upgradeSaveData.PurchasedUpgrades[upgradeKey] = newCount;
            _checksContainer.ToggleBox(newCount - 1);

            gameSaveData.TotalCoins -= _currentItem.cost;

            SaveSystem.SaveData(gameSaveData);
            SaveSystem.SaveUpgrades(upgradeSaveData);
        }


        private void OnRefund()
        {
            if (_currentItem == null || _currentItem.strategy == null)
            {
                GD.PrintErr("Refund failed: Current item or strategy is null.");
                return;
            }
            gameSaveData = SaveSystem.LoadData();
            upgradeSaveData = SaveSystem.LoadUpgrades();
            string upgradeKey = _currentItem.strategy.ResourcePath;

            upgradeSaveData.PurchasedUpgrades.TryGetValue(upgradeKey, out int currentCount);
            if (currentCount <= 0)
            {
                GD.Print("No Stacks");
                return;
            }
            gameSaveData.TotalCoins += _currentItem.cost * currentCount;

            upgradeSaveData.PurchasedUpgrades[upgradeKey] = 0;
            _checksContainer.ToggleAllBoxes(false);

            SaveSystem.SaveData(gameSaveData);
            SaveSystem.SaveUpgrades(upgradeSaveData);
        }

    }
}