using Godot;
using System;
using Godot.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
public partial class Interactions : Area2D
{
    Array<Area2D> interactablesInRange;
    InteractableComponent currentIntractable;
    Label interactLabel;
    bool CanInteract = true;
    public override void _Ready()
    {
        interactLabel = GetNode<Label>("Label");
        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;
    }

    public override void _Process(double delta)
    {
        interactablesInRange = GetOverlappingAreas();
        if (interactablesInRange.Count > 0 && CanInteract)
        {
            interactablesInRange.OrderBy(area => GlobalPosition.DistanceTo(area.GlobalPosition));
            currentIntractable = interactablesInRange[0] as InteractableComponent;
            if (currentIntractable.isInteractable)
            {
                interactLabel.Text = currentIntractable.interactName;
                interactLabel.Show();
            }
        }
        else
        {
            interactLabel.Hide();
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("interact"))
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

    private void OnAreaEntered(Area2D area)
    {
        interactablesInRange.Add(area);
    }
    private void OnAreaExited(Area2D area)
    {
        interactablesInRange.Remove(area);
    }

}
