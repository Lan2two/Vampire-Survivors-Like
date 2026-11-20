using Godot;
using System;

public partial class States : Node
{
    //Big signal action
    public event Action<States, string> stateChanged;
    //functions for states
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update(double delta) { }
    public virtual void PhysicsUpdate(double delta) { }

    protected void ChangeState(string newStateName)
    {
        stateChanged?.Invoke(this, newStateName);
    }
}
