namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class ItemDisplay : Panel
    {
        private GridContainer _gridContainer;
        private Label _costLabel;
        private TextureRect _textureRect;
        public override void _Ready()
        {
            _gridContainer = GetNodeOrNull<GridContainer>("%GridContainer");
            _costLabel = GetNodeOrNull<Label>("%Label");
            _textureRect = GetNodeOrNull<TextureRect>("%TextureRect");
            foreach (ShopItem item in _gridContainer.GetChildren())
            {
                item.ShowDetail += ShowDetail;
            }
        }

        private void ShowDetail(ShopItem item)
        {
            GD.Print($"{item.cost}");
            _costLabel.Text = $"{item.cost}";
            _textureRect.Texture = (Texture2D)item.texture;
        }

    }
}