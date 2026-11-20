namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class WorldItem : Node2D
    {
        [Export] public ItemData ItemData;
        InteractableComponent interactableComponent;
        Sprite2D sprite2D;

        public override void _Ready()
        {
            if (Engine.IsEditorHint()) return;
            interactableComponent = GetNodeOrNull<InteractableComponent>("InteractableComponent");
            sprite2D = GetNodeOrNull<Sprite2D>("Sprite2D");
            if (interactableComponent == null)
            {
                GD.PrintErr("InteractableComponent not found in WorldItem");
                return;
            }
            if (sprite2D == null)
            {
                GD.PrintErr("Sprite2D not found in WorldItem");
            }
            interactableComponent.Interact += OnInteract;
            sprite2D.Texture = ItemData.ItemIcon;
            interactableComponent.interactName = $"Pick up {ItemData.ItemName} [F]";

        }

        public override void _ExitTree()
        {
            if (interactableComponent != null)
            {
                interactableComponent.Interact -= OnInteract;
            }
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
}
