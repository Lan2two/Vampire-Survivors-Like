namespace SurvivorsGame
{
    using Godot;
    public class Attack
    {
        public float Damage { get; set; } = 0;
        public Vector2 GlobalPosition { get; set; } = Vector2.Zero;
        public float KnockbackForce { get; set; } = 0f;
        public bool Crit { get; set; } = false;
        public Color HitFlash = default;
        public float FlashDuration = 0.1f;
    }
}
