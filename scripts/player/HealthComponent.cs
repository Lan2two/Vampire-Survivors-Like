using Godot;
using System;

[GlobalClass]
public partial class HealthComponent : Node
{
    [Export] int maxHealth = 100;
    int currentHealth;

    public override void _Ready()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        GD.Print("Took damage: " + damage + ", Current Health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GD.Print("Player has died.");
        // Handle death logic here (e.g., respawn, game over screen, etc.)
        GetParent().QueueFree(); // Example: remove the player from the scene
    }
}
