namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class ShopItem : Control
    {
        [Export] public int cost { get; set; } = 10;
        [Export] public int count { get; set; } = 3;
        [Export] public Strategy strategy;
        public event Action<ShopItem> ShowDetail;
        private bool hovering = false;
        public TextureRect textureRect;
        private Label _priceLabel;
        private Panel _panel;

        public override void _Ready()
        {
            textureRect = GetNodeOrNull<TextureRect>("%TextureRect");
            _priceLabel = GetNodeOrNull<Label>("Panel/Panel/TextureRect/Label");
            _panel = GetNodeOrNull<Panel>("Panel");
            if (textureRect == null)
            {
                GD.Print("Texture not found");
            }
            _priceLabel.Text = $"{cost}";
            if (strategy == null)
            {
                GD.PrintErr("Strategy unset in ShopItem");
            }
            if (strategy.ItemIcon != null)
            {
                textureRect.Texture = strategy.ItemIcon;
            }
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