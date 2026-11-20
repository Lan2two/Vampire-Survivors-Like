using Godot;
using System;

public partial class Bullet : Node2D
{
    [Export] float speed = 700f;
    [Export] float range = 500f;
    private float traveledDistance = 0;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Vector2.Left.Rotated(Rotation);
        Position += direction * speed * (float)delta;
        traveledDistance += speed * (float)delta;
        if (range < traveledDistance)
        {
            QueueFree();
        }
    }



}
