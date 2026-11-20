using Godot;

[GlobalClass]
public partial class DamageBulletStrategy : Strategy, IStrategy
{
    public void ApplyUpgrade(Node2D node2D)
    {
        if (node2D is Bullet bullet)
        {
            bullet.config.Speed += 1f;
        }
    }
}