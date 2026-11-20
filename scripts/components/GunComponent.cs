using Godot;
using Godot.Collections;
using System;
using System.Linq;

[GlobalClass]
public partial class GunComponent : Area2D
{
    [Export] float gunTimer = 1;
    Timer timer;
    Marker2D ShootPoint;
    Array<Node2D> enemiesInRange;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        ShootPoint = GetNode<Marker2D>("%ShootPoint");
        timer.WaitTime = gunTimer;
        timer.Timeout += Shoot;
    }

    public override void _PhysicsProcess(double delta)
    {
        enemiesInRange = GetOverlappingBodies();
        if (enemiesInRange.Count > 0)
        {
            Node2D targetEnemy = enemiesInRange.First();
            LookAt(targetEnemy.GlobalPosition);
        }
    }

    private void Shoot()
    {
        if (enemiesInRange.Count > 0)
        {
            PackedScene BULLET = GD.Load<PackedScene>("uid://d1ufo1hep2ntf");
            Bullet newBullet = BULLET.Instantiate() as Bullet;
            newBullet.GlobalPosition = ShootPoint.GlobalPosition;
            newBullet.GlobalRotation = ShootPoint.GlobalRotation;
            ShootPoint.AddChild(newBullet);
        }
    }
}
