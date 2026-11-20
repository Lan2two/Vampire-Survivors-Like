using Godot;

[GlobalClass]
public partial class KnockbackStrategy : Strategy, IStrategy
{
    [Export] float UpgradeKnockback { get; set; } = 10f;
    public void ApplyUpgrade(Resource config)
    {
        if (config is IDamageDealing damageable)
        {
            damageable.Knockback += UpgradeKnockback;
        }
    }
}