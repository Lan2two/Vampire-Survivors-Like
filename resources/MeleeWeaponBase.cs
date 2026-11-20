using Godot;

[GlobalClass]
public partial class MeleeWeaponBase : WeaponBase, IDamageDealing
{
    [Export] public float SwingSpeedMultiplier { get; set; } = 1.0f;
    [Export] public float swingCooldown { get; set; } = 1.0f;

    public float Damage { get => BaseDamage; set => BaseDamage = value; }
    public float Knockback { get => BaseKnockback; set => BaseKnockback = value; }
}