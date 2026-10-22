namespace SurvivorsGame
{
    using Godot;
    public partial class Attack
    {
        public float FinalDamage { get; set; } = 0;
        public float BaseDamage { get; set; } = 0;
        public Vector2 GlobalPosition { get; set; } = Vector2.Zero;
        public float KnockbackForce { get; set; } = 0f;
        public float CritRate { get; set; } = 0f;
        public float CritDamageMultiplier { get; set; } = 1.2f;
        public bool Crit { get; set; } = false;
    }
}
