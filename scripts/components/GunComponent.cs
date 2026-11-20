using Godot;
using Godot.Collections;
using System;
using System.Linq;

[GlobalClass]
public partial class GunComponent : Area2D
{
    [Export] float gunTimer = 1;
    UpgradeManager upgradeManager;
    Timer timer;
    Marker2D ShootPoint;
    Array<Node2D> enemiesInRange;
    PackedScene BULLET;


    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        ShootPoint = GetNode<Marker2D>("%ShootPoint");
        upgradeManager = GetParent().GetNode<UpgradeManager>("UpgradeManager");
        timer.WaitTime = gunTimer;
        timer.Timeout += Shoot;
        BULLET = GD.Load<PackedScene>("uid://d1ufo1hep2ntf");
    }

    public override void _PhysicsProcess(double delta)
    {
        enemiesInRange = GetOverlappingBodies();
        Node2D closestEnemy = null;
        float closestDistSq = float.MaxValue;
        foreach (Node2D enemy in enemiesInRange)
        {
            float distSq = GlobalPosition.DistanceSquaredTo(enemy.GlobalPosition);
            if (distSq < closestDistSq)
            {
                closestDistSq = distSq;
                closestEnemy = enemy;
            }
        }
        if (closestEnemy != null)
        {
            LookAt(closestEnemy.GlobalPosition);
        }
    }

    private void Shoot()
    {
        if (enemiesInRange.Count > 0)
        {
            Bullet newBullet = BULLET.Instantiate<Bullet>();
            newBullet.Use(upgradeManager);
            newBullet.GlobalPosition = ShootPoint.GlobalPosition;
            newBullet.GlobalRotation = ShootPoint.GlobalRotation;
            ShootPoint.AddChild(newBullet);
        }
    }
}
