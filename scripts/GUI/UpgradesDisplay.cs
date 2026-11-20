namespace SurvivorsGame
{
    using Godot;
    using System.Collections.Generic;

    public partial class UpgradesDisplay : ColorRect
    {
        private GridContainer _gridContainer;
        private TextureRect _textureRect;
        private Control _parent;
        private Player _player;

        public override void _Ready()
        {
            _gridContainer = GetNodeOrNull<GridContainer>("GridContainer");
            _textureRect = GetNodeOrNull<TextureRect>("TextureRect");
            _parent = GetParent() as Control;
            _player = this.GetPlayer();
            if (_gridContainer == null)
            {
                GD.PrintErr("GridContainer not found in UpgradesDisplay");
                return;
            }
            if (_textureRect == null)
            {
                GD.PrintErr("TextureRect not found in UpgradesDisplay");
                return;
            }
            if (_parent == null)
            {
                GD.PrintErr("Parent not found in UpgradesDisplay");
                return;
            }
            if (_player == null)
            {
                GD.PrintErr("Player not found in UpgradesDisplay");
                return;
            }
            GameScene.GameOverEvent += OnGameOver;
        }

        public override void _ExitTree()
        {
            GameScene.GameOverEvent -= OnGameOver;
        }

        private void OnGameOver()
        {
            if (!IsInstanceValid(_gridContainer)
                || !IsInstanceValid(_textureRect)
                || !IsInstanceValid(_player))
            {
                return;
            }

            foreach (Node child in _gridContainer.GetChildren())
            {
                _gridContainer.RemoveChild(child);
                child.QueueFree();
            }

            List<Strategy> displayedUpgrades = new();
            foreach (Strategy strategy in _player.inventoryManager.Upgrades)
            {
                if (displayedUpgrades.Exists(displayedUpgrade => ReferenceEquals(displayedUpgrade, strategy)))
                {
                    continue;
                }

                displayedUpgrades.Add(strategy);
                int duplicateCount = 0;
                foreach (Strategy ownedUpgrade in _player.inventoryManager.Upgrades)
                {
                    if (ReferenceEquals(ownedUpgrade, strategy))
                    {
                        duplicateCount++;
                    }
                }

                TextureRect textureRect = _textureRect.Duplicate() as TextureRect;
                textureRect.Texture = strategy.ItemIcon;
                Label label = textureRect.GetChild<Label>(0);
                label.Text = $"{strategy.ItemName}\n{duplicateCount}/{strategy.UpgradePool}";
                textureRect.Visible = true;
                _gridContainer.AddChild(textureRect);
            }
        }
    }
}