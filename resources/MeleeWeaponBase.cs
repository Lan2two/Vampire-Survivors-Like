using Godot;

[GlobalClass]
public partial class MeleeWeaponBase : WeaponBase
{
    [Export] public float SwingSpeedMultiplier { get; set; } = 1.0f;
    [Export] public float swingCooldown { get; set; } = 1.0f;
}