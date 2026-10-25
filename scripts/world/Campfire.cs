namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class Campfire : Node2D
    {
        [Export] private float Duration = 60;
        [Export] private float Cooldown = 10;
        private InteractableComponent interactableComponent;
        private AttackComponent attackComponent;
        private PointLight2D pointLight2D;
        private Sprite2D sprite2D;
        private Timer cooldownTimer;
        private float _durationTimer;

        public override void _Ready()
        {
            interactableComponent = GetNodeOrNull<InteractableComponent>("InteractableComponent");
            pointLight2D = GetNodeOrNull<PointLight2D>("PointLight2D");
            attackComponent = GetNodeOrNull<AttackComponent>("AttackComponent");
            sprite2D = GetNodeOrNull<Sprite2D>("Sprite2D");
            cooldownTimer = GetNodeOrNull<Timer>("CooldownTimer");
            if (interactableComponent == null)
            {
                GD.PrintErr("InteractableComponent not found in Campfire");
                return;
            }
            if (pointLight2D == null)
            {
                GD.PrintErr("PointLight2D not found in Campfire");
                return;
            }
            if (attackComponent == null)
            {
                GD.PrintErr("AttackComponent not found in Campfire");
                return;
            }
            if (sprite2D == null)
            {
                GD.PrintErr("Sprite2D not found in Campfire");
                return;
            }
            if (cooldownTimer == null)
            {
                GD.PrintErr("CooldownTimer not found in Campfire");
            }
            cooldownTimer.WaitTime = Cooldown + Duration;
            interactableComponent.Interact += OnInteract;
            cooldownTimer.Timeout += OnTimeout;
        }

        public override void _ExitTree()
        {
            if (interactableComponent != null)
            {
                interactableComponent.Interact -= OnInteract;
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            _durationTimer -= (float)delta;
            if (_durationTimer <= 0)
            {
                SetCampfire(false);
            }
        }

        private void SetCampfire(bool set)
        {
            pointLight2D.Visible = set;
            attackComponent.SetAttackMonitoring(set);
        }
        private void OnInteract()
        {
            SetCampfire(true);
            interactableComponent.isInteractable = false;
            _durationTimer = Duration;
            cooldownTimer.Start();
        }

        private void OnTimeout()
        {
            interactableComponent.isInteractable = true;
        }
    }
}