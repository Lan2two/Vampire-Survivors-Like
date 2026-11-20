using Godot;

[GlobalClass]
public partial class DamageStrategy : Strategy, IStrategy
{
    [Export] float UpgradeDamage { get; set; } = 5f;
    public void ApplyUpgrade(Resource config)
    {
        if (config is IDamageDealing damageable)
        {
            damageable.Damage += UpgradeDamage;
        }
    }
}