using Godot;

[GlobalClass]
public partial class BulletData : Resource
{
    [Export] public float Damage = 10f;
    [Export] public float Knockback = 100f;
    [Export] public float Speed { get; set; } = 300f;
    [Export] public float MaxTravelDistance { get; set; } = 500f;
    [Export] public int PierceCount { get; set; } = 0;
}