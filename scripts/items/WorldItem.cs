using Godot;
using System;

[GlobalClass]
public partial class WorldItem : Node2D
{
    [Export] public ItemData ItemData;
    InteractableComponent interactableComponent;
    Sprite2D sprite2D;

    public override void _Ready()
    {
        if (Engine.IsEditorHint()) return;
        interactableComponent = GetNode<InteractableComponent>("InteractableComponent");
        sprite2D = GetNode<Sprite2D>("Sprite2D");
        interactableComponent.Interact += OnInteract;
        if (ItemData != null)
        {
            sprite2D.Texture = ItemData.ItemIcon;
        }
    }

    public override void _ExitTree()
    {
        interactableComponent.Interact -= OnInteract;
    }


    private void OnInteract()
    {
        bool picked = this.GetPlayer().inventoryManager.AddItem(ItemData);
        if (picked)
        {
            QueueFree();
        }
    }
}
