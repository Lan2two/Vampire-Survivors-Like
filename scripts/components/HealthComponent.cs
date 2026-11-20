using Godot;
using System;

[GlobalClass]
public partial class HealthComponent : Node
{
    [Export] float maxHealth = 100f;
    [Export] CharacterBody2D characterBody;
    [Signal] public delegate void DamageEventHandler();
    public float currentHealth;

    public override void _Ready()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(Attack attackData)
    {
        currentHealth -= attackData.Damage;
        EmitSignal(SignalName.Damage);
        GD.Print("Took damage: " + attackData.Damage + ", Current Health: " + currentHealth);
        if (characterBody != null)
        {
            TakeKnockback(attackData);
        }
    }

    private void TakeKnockback(Attack attackData)
    {
        Vector2 pushDirection = (characterBody.GlobalPosition - attackData.GlobalPosition).Normalized();
        characterBody.Velocity += pushDirection * attackData.KnockbackForce;
    }
}
