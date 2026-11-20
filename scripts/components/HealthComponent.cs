using Godot;
using System;

[GlobalClass]
public partial class HealthComponent : Node
{
    [Export] float maxHealth = 100f;

    public event Action<Attack> Damage;
    public float currentHealth;

    public override void _Ready()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(Attack attackData)
    {
        currentHealth -= attackData.Damage;
        Damage.Invoke(attackData);
        GD.Print("Took damage: " + attackData.Damage + ", Current Health: " + currentHealth);
    }


}
