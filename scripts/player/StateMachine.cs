using Godot;
using System;
using System.Collections.Generic;

public partial class StateMachine : Node
{
    Dictionary<string, States> _states = new Dictionary<string, States>();
    States currentState;
    [Export] States startingState;

    public override void _Ready()
    {
        foreach (Node child in GetChildren())
        {
            if (child is States state)
            {
                _states.Add(state.Name, state);
            }
        }
        if (startingState != null)
        {
            currentState = startingState;
            currentState.Enter();
        }
        GD.Print("Current State: " + currentState.Name);
    }
}
