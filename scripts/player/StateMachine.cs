using Godot;
using System;
using System.Collections.Generic;

public partial class StateMachine : Node
{
    Dictionary<string, States> _states = new();
    States currentState;
    [Export] States startingState;

    public override void _Ready()
    {
        foreach (Node child in GetChildren())
        {
            if (child is States state)
            {
                string stateName = state.Name.ToString().ToLower();
                _states.Add(stateName, state);
                GD.Print("Registered State: " + stateName);
                state.Connect(nameof(States.StateChanged), new Callable(this, nameof(OnStateChanged)));
            }
        }
        if (startingState != null)
        {
            currentState = startingState;
            currentState.Enter();
        }
        GD.Print("Current State: " + currentState.Name);
    }

    public override void _Process(double delta)
    {
        currentState?.Update(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        currentState?.PhysicsUpdate(delta);
    }

    private void OnStateChanged(string newStateName)
    {
        if (currentState == null) return;
        string targetStateName = newStateName.ToLower();
        if (!_states.TryGetValue(targetStateName, out States newState))
        {
            GD.PrintErr("State not found: " + targetStateName);
            return;
        }
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
        GD.Print("Changed State: " + currentState.Name);
    }
}
