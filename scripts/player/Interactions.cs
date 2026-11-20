using Godot;
using System;
using Godot.Collections;
using System.Linq;

public partial class Interactions : Area2D
{
    InteractableComponent currentIntractable;
    Label interactLabel;
    bool CanInteract = true;

    public override void _Ready()
    {
        interactLabel = GetNode<Label>("Label");
    }

    public override void _Process(double delta)
    {
        Array<Area2D> interactablesInRange = GetOverlappingAreas();

        if (interactablesInRange.Count > 0 && CanInteract)
        {
            currentIntractable = interactablesInRange
                .OrderBy(area => GlobalPosition.DistanceTo(area.GlobalPosition))
                .FirstOrDefault() as InteractableComponent;

            if (currentIntractable != null && currentIntractable.isInteractable)
            {
                interactLabel.Text = currentIntractable.interactName;
                interactLabel.Show();
            }
            else
            {
                interactLabel.Hide();
            }
        }
        else
        {
            currentIntractable = null;
            interactLabel.Hide();
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("interact") && IsInstanceValid(currentIntractable))
        {
            currentIntractable.EmitSignal(InteractableComponent.SignalName.Interact);
        }
    }

    private async void OnInteract()
    {
        CanInteract = false;
        interactLabel.Hide();
        await ToSignal(currentIntractable, InteractableComponent.SignalName.Interact);
        CanInteract = true;
    }
}
