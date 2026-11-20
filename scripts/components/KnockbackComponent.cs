using Godot;
using System;

[GlobalClass]
public partial class KnockbackComponent : Node
{
    [Export] HealthComponent healthComponent;
    [Export] VelocityComponent velocityComponent;
    [Export] CharacterBody2D characterBody;
    public override void _Ready()
    {
        if (velocityComponent == null)
        {
            GD.PrintErr("VelocityComponent unset in Knockback");
        }
        healthComponent.Damage += TakeKnockback;
    }
    public override void _ExitTree()
    {
        healthComponent.Damage -= TakeKnockback;
    }

    private void TakeKnockback(Attack attackData)
    {
        if (velocityComponent == null)
        {
            return;
        }

        Vector2 pushDirection = (characterBody.GlobalPosition - attackData.GlobalPosition).Normalized();
        Vector2 knockbackVelocity = pushDirection * attackData.KnockbackForce;

        if (velocityComponent != null)
        {
            velocityComponent.Velocity += knockbackVelocity;
        }
    }

}
