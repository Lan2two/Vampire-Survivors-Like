using Godot;

[GlobalClass]
public partial class BulletData : Resource, IDamageDealing
{
    [Export] public float Damage { get; set; } = 10f;
    [Export] public float Knockback { get; set; } = 100f;
    [Export] public float Speed { get; set; } = 300f;
    [Export] public float MaxTravelDistance { get; set; } = 500f;
    [Export] public int PierceCount { get; set; } = 0;
}