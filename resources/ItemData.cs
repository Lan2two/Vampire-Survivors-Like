namespace SurvivorsGame
{
    using Godot;
    [GlobalClass]
    public partial class ItemData : Resource
    {
        [Export] public string ItemName { get; set; } = "";
        [Export] public string ItemDescription { get; set; } = "";
        [Export] public Texture2D ItemIcon { get; set; } = GD.Load<Texture2D>("res://icon.svg");
        [Export] public int MaxStackSize { get; set; } = 1;

    }
}
