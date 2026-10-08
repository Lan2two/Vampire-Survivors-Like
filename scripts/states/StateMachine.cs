using Godot;
using System;
using System.Collections.Generic;

public partial class StateMachine : Node
{
    Dictionary<string, States> _states = new();
    Dictionary<string, Interrupt> _interrupts = new();
    States currentState;
    [Export] States startingState;
    public AnimatedSprite2D anim;

    public override void _Ready()
    {
        foreach (Node child in GetChildren())
        {
            if (child is Interrupt interrupt)
            {
                string interruptName = interrupt.Name.ToString().ToLower();
                _interrupts.Add(interruptName, interrupt);
                interrupt.Interrupted += OnInterruptTriggered;
            }
            if (child is States state)
            {
                string stateName = state.Name.ToString().ToLower();
                _states.Add(stateName, state);
                //signal
                state.stateChanged += OnStateChanged;
            }
        }
        if (startingState != null)
        {
            currentState = startingState;
            CallDeferred(nameof(EnterCurrentState));
        }

    }

    private void EnterCurrentState()
    {
        currentState?.Enter();
    }

    public override void _Process(double delta)
    {
        currentState?.Update(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        currentState?.PhysicsUpdate(delta);
    }

    private void OnInterruptTriggered(Interrupt interrupt, string interruptName)
    {
        ChangeState(interruptName);
    }

    private void OnStateChanged(States callingState, string newStateName)
    {
        if (callingState != currentState) return;
        ChangeState(newStateName);
    }

    private void ChangeState(string newStateName)
    {
        string targetStateName = newStateName.ToLower();
        if (!_states.TryGetValue(targetStateName, out States newState))
        {
            GD.PrintErr("State not found: " + targetStateName);
            return;
        }
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public override void _ExitTree()
    {
        foreach (States state in _states.Values)
        {
            state.stateChanged -= OnStateChanged;
        }
        foreach (Interrupt interrupt in _interrupts.Values)
        {
            interrupt.Interrupted -= OnInterruptTriggered;
        }
    }
}

