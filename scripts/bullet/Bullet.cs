using Godot;
using System;

public partial class Bullet : Node2D
{
    [Export] BulletData config;
    [Export] DamageComponent damageComponent;
    private float traveledDistance = 0;
    private float timer = 1;
    public override void _Ready()
    {
        if (damageComponent == null)
        {
            GD.Print("DamageComponent unassigned in Bullet");
            return;
        }

        damageComponent.MaxHits = config.PierceCount + 1;
        if (config.PierceCount > 0)
        {
            damageComponent.SingleHit = false;
        }

        damageComponent.HitLimitReached += OnHitLimitReached;
    }

    public override void _ExitTree()
    {
        if (damageComponent != null)
        {
            damageComponent.HitLimitReached -= OnHitLimitReached;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Vector2.Left.Rotated(Rotation).Normalized();
        Position += direction * config.Speed * (float)delta;
        traveledDistance += config.Speed * (float)delta;

        if (config.MaxTravelDistance < traveledDistance)
        {
            QueueFree();
        }
    }

    private void OnHitLimitReached()
    {
        QueueFree();
    }
}
