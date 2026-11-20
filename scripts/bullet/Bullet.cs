using Godot;
using System;

public partial class Bullet : Node2D
{
    [Export] BulletData config;
    [Export] DamageComponent damageComponent;
    private float traveledDistance = 0;
    private int hitCount = 0;
    public override void _Ready()
    {
        if (damageComponent == null)
        {
            GD.Print("DamageComponent unassigned in Bullet");
            return;
        }

        damageComponent.AreaEntered += OnAreaEntered;
    }

    public override void _ExitTree()
    {
        if (damageComponent != null)
        {
            damageComponent.AreaEntered -= OnAreaEntered;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Vector2.Left.Rotated(Rotation).Normalized();
        Position += direction * config.Speed * (float)delta;
        traveledDistance += config.Speed * (float)delta;

        if (config.MaxTravelDistance < traveledDistance || hitCount > config.PierceCount)
        {
            QueueFree();
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is HitboxComponent)
        {
            hitCount += 1;
        }
    }
}
