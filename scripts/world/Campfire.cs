namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class Campfire : Node2D
    {
        [Export] private float Duration;
        [Export] private float Cooldown;
        private InteractableComponent interactableComponent;
        private PointLight2D pointLight2D;
        private float _durationTimer;
        private float _cooldownTimer;

        public override void _Ready()
        {
            interactableComponent = GetNodeOrNull<InteractableComponent>("InteractableComponent");
            pointLight2D = GetNodeOrNull<PointLight2D>("PointLight2D");
            if (interactableComponent == null)
            {
                GD.PrintErr("InteractableComponent not found in Campfire");
            }
            if (pointLight2D == null)
            {
                GD.PrintErr("PointLight2D not found in Campfire");
            }
            interactableComponent.Interact += OnInteract;
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
            pointLight2D.Visible = true;
        }

    }
}