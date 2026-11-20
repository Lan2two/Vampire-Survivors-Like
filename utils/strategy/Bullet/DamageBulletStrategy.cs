using Godot;

[GlobalClass]
public partial class DamageBulletStrategy : Strategy, IStrategy
{
    [Export] float UpgradeDamage { get; set; } = 5f;
    public void ApplyUpgrade(Node2D node2D)
    {
        if (node2D is Bullet bullet)
        {
            bullet.config.Damage += UpgradeDamage;
        }
    }

}