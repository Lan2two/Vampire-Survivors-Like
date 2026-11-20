namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class ShopItem : Control
    {
        [Export]
        public int cost { get; set; } = 10;
        public event Action<ShopItem> ShowDetail;
        private bool hovering = false;
        public Texture texture;
        private Label _priceLabel;
        private Panel _panel;

        public override void _Ready()
        {
            texture = GetNodeOrNull<TextureRect>("Panel/Panel/TextureRect").Texture;
            _priceLabel = GetNodeOrNull<Label>("Panel/Panel/TextureRect/Label");
            _panel = GetNodeOrNull<Panel>("Panel");
            if (texture == null)
            {
                GD.Print("Texture not found");
            }
            _priceLabel.Text = $"{cost}";
            MouseEntered += OnMouseEntered;
            MouseExited += OnMouseExited;
        }

        public override void _GuiInput(InputEvent @event)
        {
            if (@event is InputEventMouseButton mouseEvent)
            {
                if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
                {
                    GD.Print("Clicked");
                    ShowDetail?.Invoke(this);
                }
            }
        }

        private void OnMouseEntered()
        {
            _panel.SelfModulate = Colors.Black;
        }

        private void OnMouseExited()
        {
            _panel.SelfModulate = Colors.White;
        }

    }
}