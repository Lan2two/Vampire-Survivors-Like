using Godot;
using System;

[GlobalClass]
public partial class HealthComponent : Node
{
    [Export] float maxHealth = 100f;
    [Export] VelocityComponent velocityComponent;
    [Export] CharacterBody2D characterBody;
    [Signal] public delegate void DamageEventHandler();
    public float currentHealth;

    public override void _Ready()
    {
        currentHealth = maxHealth;
        if (velocityComponent == null)
        {
            GD.PrintErr("VelocityComponent unset in Health");
        }
    }

    public void TakeDamage(Attack attackData)
    {
        currentHealth -= attackData.Damage;
        EmitSignal(SignalName.Damage);
        GD.Print("Took damage: " + attackData.Damage + ", Current Health: " + currentHealth);
        if (velocityComponent != null)
        {
            TakeKnockback(attackData);
        }
    }

    private void TakeKnockback(Attack attackData)
    {
        if (velocityComponent == null)
        {
            return;
        }

        Vector2 pushDirection = (characterBody.GlobalPosition - attackData.GlobalPosition).Normalized();
        Vector2 knockbackVelocity = pushDirection * attackData.KnockbackForce;

        characterBody.Velocity += knockbackVelocity;

        if (velocityComponent != null)
        {
            velocityComponent.Velocity += knockbackVelocity;
        }
    }
}
