using Godot;
using System;

public partial class Bullet : Area2D
{
    [Export] public BulletData config;
    private float traveledDistance = 0;
    private int hitCount = 0;
    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    public override void _ExitTree()
    {
        AreaEntered -= OnAreaEntered;
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
        if (area is HitboxComponent hitbox)
        {
            Attack attack = new Attack
            {
                Damage = config.Damage,
                GlobalPosition = GlobalPosition,
                KnockbackForce = config.Knockback
            };
            hitbox.TakeDamage(attack);
            hitCount += 1;
        }
    }
}
