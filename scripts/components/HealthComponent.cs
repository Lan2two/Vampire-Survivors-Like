using Godot;
using System;

[GlobalClass]
public partial class HealthComponent : Node
{
    [Export] float maxHealth = 100f;

    [Signal] public delegate void DamageEventHandler(Attack attackData);
    public float currentHealth;

    public override void _Ready()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(Attack attackData)
    {
        currentHealth -= attackData.Damage;
        EmitSignal(SignalName.Damage, attackData);
        GD.Print("Took damage: " + attackData.Damage + ", Current Health: " + currentHealth);
    }


}
