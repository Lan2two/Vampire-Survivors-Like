using Godot;
using System;

public partial class Bullet : Node2D
{
    [Export] BulletData config;
    [Export] DamageComponent damageComponent;
    private float traveledDistance = 0;
    public override void _Ready()
    {
        if (damageComponent == null)
        {
            GD.Print("DamageComponent unassigned in Bullet");
        }
        if (damageComponent != null && config.PierceCount > 0)
        {
            damageComponent.SingleHit = false;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Vector2.Left.Rotated(Rotation).Normalized();
        Position += direction * config.Speed * (float)delta;
        traveledDistance += config.Speed * (float)delta;
        if (config.MaxTravelDistance < traveledDistance || damageComponent.UniqueHitboxesEntered > config.PierceCount)
        {
            QueueFree();
        }
    }



}
