using Godot;
using System;

public partial class Equipable : Node2D
{
    [Export] Texture2D texture;
    [Export] PackedScene weapon;
    InteractableComponent interactableComponent;
    Sprite2D sprite2D;

    public override void _Ready()
    {
        interactableComponent = GetNode<InteractableComponent>("InteractableComponent");
        sprite2D = GetNode<Sprite2D>("Sprite2D");
        sprite2D.Texture = texture;
        interactableComponent.Interact += OnInteract;
    }

    public override void _ExitTree()
    {
        interactableComponent.Interact -= OnInteract;
    }


    private void OnInteract()
    {
        this.GetPlayer().inventoryManager.SwitchWeapon(weapon);
        QueueFree();
    }
}
