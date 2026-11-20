using Godot;
[GlobalClass]
public partial class ItemData : Resource
{
    [Export] public string ItemName { get; set; } = "Item";
    [Export] public Texture2D ItemIcon { get; set; }
    [Export] public int MaxStackSize { get; set; } = 1;

    public virtual void Use()
    {

    }


}