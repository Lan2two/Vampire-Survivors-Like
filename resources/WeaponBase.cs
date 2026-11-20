using Godot;
[GlobalClass]
public partial class WeaponBase : ItemData
{
    [Export] public float BaseDamage { get; set; } = 10;
    [Export] public float BaseKnockback { get; set; } = 100f;
    [Export] public PackedScene WeaponVisualScene;
}