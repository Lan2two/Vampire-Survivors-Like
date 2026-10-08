using Godot;
using System;

[GlobalClass]

public partial class Interrupt : States
{
    public event Action<Interrupt, string> Interrupted;

    protected void FireInterrupt()
    {
        Interrupted?.Invoke(this, Name);
    }
}