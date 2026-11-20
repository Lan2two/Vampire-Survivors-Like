using Godot;
using System;

[GlobalClass]
public partial class HealthComponent : Node
{
    [Export] int maxHealth = 100;
    [Signal] public delegate void DamageEventHandler();
    [Signal] public delegate void DeathEventHandler();
    int currentHealth;

    public override void _Ready()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        EmitSignal(SignalName.Damage);
        GD.Print("Took damage: " + damage + ", Current Health: " + currentHealth);
        if (currentHealth <= 0)
        {
            EmitSignal(SignalName.Death);
        }
    }
}
