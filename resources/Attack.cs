using Godot;

[GlobalClass]
public partial class Attack : Resource
{
    public float Damage { get; set; } = 0;
    public Vector2 GlobalPosition { get; set; } = Vector2.Zero;
    public float KnockbackForce { get; set; } = 0f;
}