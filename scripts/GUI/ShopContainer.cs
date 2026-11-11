namespace SurvivorsGame
{
    using Godot;
    public partial class ShopContainer : SubMenuContainer
    {
        private GridContainer _gridContainer;
        private Label _costLabel;
        private TextureRect _textureRect;
        private Panel _itemDisplay;
        public override void _Ready()
        {
            base._Ready();
            _gridContainer = GetNodeOrNull<GridContainer>("%GridContainer");
            _costLabel = GetNodeOrNull<Label>("%Label");
            _textureRect = GetNodeOrNull<TextureRect>("%TextureRect");
            _itemDisplay = GetNodeOrNull<Panel>("Panel/ItemDisplay");
            foreach (ShopItem item in _gridContainer.GetChildren())
            {
                item.ShowDetail += ShowDetail;
            }
        }

        private void ShowDetail(ShopItem item)
        {
            _itemDisplay.Visible = true;
            _costLabel.Text = $"{item.cost}";
            _textureRect.Texture = (Texture2D)item.texture;
        }

    }
}