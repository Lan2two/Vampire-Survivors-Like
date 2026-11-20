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
        private GameSaveData gameSaveData;
        private UpgradeSaveData upgradeSaveData;
        SaveSystem saveSystem;
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

            saveSystem = new SaveSystem();

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
        }

        private void OnUpgrade()
        {
            if (_currentItem == null || _currentItem.strategy == null)
            {
                GD.PrintErr("Upgrade failed: Current item or strategy is null.");
                return;
            }

            gameSaveData = saveSystem.LoadData();

            if (gameSaveData.TotalCoins < _currentItem.cost)
            {
                GD.Print("Not enough coins");
                return;
            }

            upgradeSaveData = saveSystem.LoadUpgrades();
            string upgradeKey = _currentItem.strategy.ResourcePath;

            upgradeSaveData.PurchasedUpgrades.TryGetValue(upgradeKey, out int currentCount);

            if (currentCount >= _currentItem.count)
            {
                GD.Print("Max Stacks");
                return;
            }

            int newCount = currentCount + 1;
            upgradeSaveData.PurchasedUpgrades[upgradeKey] = newCount;
            GD.Print($"Upgrade: {_currentItem.strategy.ItemName}, Count: {newCount}");

            gameSaveData.TotalCoins -= _currentItem.cost;
            GD.Print($"Remaining Coins: {gameSaveData.TotalCoins}");

            saveSystem.SaveData(gameSaveData);
            saveSystem.SaveUpgrades(upgradeSaveData);
        }


        private void OnRefund()
        {
            if (_currentItem == null || _currentItem.strategy == null)
            {
                GD.PrintErr("Refund failed: Current item or strategy is null.");
                return;
            }
            gameSaveData = saveSystem.LoadData();
            upgradeSaveData = saveSystem.LoadUpgrades();
            string upgradeKey = _currentItem.strategy.ResourcePath;

            upgradeSaveData.PurchasedUpgrades.TryGetValue(upgradeKey, out int currentCount);
            if (currentCount <= 0)
            {
                GD.Print("No Stacks");
                return;
            }
            gameSaveData.TotalCoins += _currentItem.cost * currentCount;
            GD.Print($"Remaining Coins: {gameSaveData.TotalCoins}");

            upgradeSaveData.PurchasedUpgrades[upgradeKey] = 0;
            GD.Print($"Upgrade: {_currentItem.strategy.ItemName}, Count: {upgradeSaveData.PurchasedUpgrades[upgradeKey]}");

            saveSystem.SaveData(gameSaveData);
            saveSystem.SaveUpgrades(upgradeSaveData);
        }

    }
}