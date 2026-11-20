using Godot;

public partial class Attack
{
    public float Damage { get; set; } = 0;
    public Vector2 GlobalPosition { get; set; } = Vector2.Zero;
    public float KnockbackForce { get; set; } = 0f;
}