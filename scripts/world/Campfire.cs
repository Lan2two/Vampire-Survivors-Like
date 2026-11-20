namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class Campfire : StaticBody2D
    {
        [Export] private float Duration = 60;
        [Export] private float Cooldown = 10;
        private InteractableComponent interactableComponent;
        private AttackComponent attackComponent;
        private PointLight2D pointLight2D;
        private AnimatedSprite2D animatedSprite2D;
        private Timer _cooldownTimer;
        private Timer _durationTimer;
        private Label label;

        public override void _Ready()
        {
            interactableComponent = GetNodeOrNull<InteractableComponent>("InteractableComponent");
            pointLight2D = GetNodeOrNull<PointLight2D>("PointLight2D");
            attackComponent = GetNodeOrNull<AttackComponent>("AttackComponent");
            animatedSprite2D = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
            _cooldownTimer = GetNodeOrNull<Timer>("CooldownTimer");
            _durationTimer = GetNodeOrNull<Timer>("DurationTimer");
            label = GetNodeOrNull<Label>("Label");
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
            if (animatedSprite2D == null)
            {
                GD.PrintErr("Sprite2D not found in Campfire");
                return;
            }
            if (_cooldownTimer == null)
            {
                GD.PrintErr("CooldownTimer not found in Campfire");
            }
            if (_durationTimer == null)
            {
                GD.PrintErr("DurationTimer not found in Campfire");
            }
            _cooldownTimer.WaitTime = Cooldown + Duration;
            _durationTimer.WaitTime = Duration;
            interactableComponent.Interact += OnInteract;
            _cooldownTimer.Timeout += OnCooldownTimeout;
            _durationTimer.Timeout += OnDurationTimeout;
        }

        public override void _Process(double delta)
        {
            label.Text = $"{_durationTimer.TimeLeft}";
        }


        public override void _ExitTree()
        {
            if (interactableComponent != null)
            {
                interactableComponent.Interact -= OnInteract;
            }
            if (_cooldownTimer != null)
            {
                _cooldownTimer.Timeout -= OnCooldownTimeout;
            }
            if (_durationTimer != null)
            {
                _durationTimer.Timeout -= OnDurationTimeout;
            }
        }

        private void SetCampfire(bool set)
        {
            pointLight2D.Visible = set;
            attackComponent.SetAttackMonitoring(set);
            if (set)
            {
                animatedSprite2D.Play("lit");
            }
            else
            {
                animatedSprite2D.Play("default");
            }
        }
        private void OnInteract()
        {
            SetCampfire(true);
            interactableComponent.isInteractable = false;
            _durationTimer.Start();
            _cooldownTimer.Start();
        }

        private void OnCooldownTimeout()
        {
            interactableComponent.isInteractable = true;
        }
        private void OnDurationTimeout()
        {
            SetCampfire(false);
        }
    }
}