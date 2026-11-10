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

        public override void _Ready()
        {
            texture = GetNodeOrNull<TextureRect>("Panel/Panel/TextureRect").Texture;
            _priceLabel = GetNodeOrNull<Label>("Panel/Panel/TextureRect/Label");
            if (texture == null)
            {
                GD.Print("Texture not found");
            }
            _priceLabel.Text = $"{cost}";
            MouseEntered += OnMouseEntered;
        }

        private void OnMouseEntered()
        {
            ShowDetail?.Invoke(this);
        }

    }
}