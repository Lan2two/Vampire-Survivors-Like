namespace SurvivorsGame
{
    using Godot;
    using System.Collections.Generic;

    public partial class Interactions : Area2D
    {
        private readonly List<InteractableComponent> _nearby = new();
        private InteractableComponent _currentInteractable;
        private Label _interactLabel;
        private bool _canInteract = true;

        public override void _Ready()
        {
            _interactLabel = GetNode<Label>("Label");
            _interactLabel.Text = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789[]";
            _interactLabel.Modulate = Colors.Transparent;
            _interactLabel.Show();

            AreaEntered += OnAreaEntered;
            AreaExited += OnAreaExited;
        }

        public override void _ExitTree()
        {
            AreaEntered -= OnAreaEntered;
            AreaExited -= OnAreaExited;
        }

        private void OnAreaEntered(Area2D area)
        {
            if (area is InteractableComponent interactable && !_nearby.Contains(interactable))
            {
                _nearby.Add(interactable);
            }
        }

        private void OnAreaExited(Area2D area)
        {
            if (area is InteractableComponent interactable)
            {
                _nearby.Remove(interactable);
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            if (!_canInteract || _nearby.Count == 0)
            {
                SetCurrentInteractable(null);
                return;
            }

            InteractableComponent closest = null;
            float closestDistSq = float.MaxValue;
            Vector2 myPos = GlobalPosition;

            for (int i = _nearby.Count - 1; i >= 0; i--)
            {
                var item = _nearby[i];
                if (!IsInstanceValid(item))
                {
                    _nearby.RemoveAt(i);
                    continue;
                }

                if (!item.isInteractable) continue;

                float distSq = myPos.DistanceSquaredTo(item.GlobalPosition);
                if (distSq < closestDistSq)
                {
                    closestDistSq = distSq;
                    closest = item;
                }
            }

            SetCurrentInteractable(closest);
        }

        private void SetCurrentInteractable(InteractableComponent target)
        {
            if (_currentInteractable == target)
            {
                if (target != null && _interactLabel.Text != target.interactName)
                {
                    _interactLabel.Text = target.interactName;
                }
                return;
            }

            _currentInteractable = target;

            if (_currentInteractable != null)
            {
                _interactLabel.Text = _currentInteractable.interactName;
                _interactLabel.Modulate = Colors.White;
            }
            else
            {
                _interactLabel.Modulate = Colors.Transparent;
            }
        }

        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed("interact") && IsInstanceValid(_currentInteractable))
            {
                if (_currentInteractable.isInteractable && _canInteract)
                {
                    GetViewport().SetInputAsHandled();
                    OnInteract();
                    _currentInteractable.EmitSignal(InteractableComponent.SignalName.Interact);
                }
            }
        }

        private async void OnInteract()
        {
            _canInteract = false;
            _interactLabel.Modulate = Colors.Transparent;
            await ToSignal(_currentInteractable, InteractableComponent.SignalName.Interact);
            _canInteract = true;
        }
    }
}
