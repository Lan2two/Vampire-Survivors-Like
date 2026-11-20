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
            Bullet newBullet = BULLET.Instantiate<Bullet>();
            if (newBullet.config != null)
            {
                newBullet.config = (BulletData)newBullet.config.Duplicate();
            }
            else
            {
                newBullet.config = new BulletData();
            }
            newBullet.GlobalPosition = ShootPoint.GlobalPosition;
            newBullet.GlobalRotation = ShootPoint.GlobalRotation;
            ShootPoint.AddChild(newBullet);
            upgradeManager.UpgradeBullet(newBullet);
        }
    }
}
