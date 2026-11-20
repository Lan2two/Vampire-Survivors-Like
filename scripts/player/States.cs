using Godot;
using System;

public partial class States : Node
{
    //Big signal action
    [Signal]
    public delegate void StateChangedEventHandler(string newState);
    //functions for states
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update(double delta) { }
    public virtual void PhysicsUpdate(double delta) { }

    protected void ChangeState(string newStateName)
    {
        EmitSignal(nameof(StateChanged), newStateName);
    }
}
