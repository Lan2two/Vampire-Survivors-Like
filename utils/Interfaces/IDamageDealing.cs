namespace SurvivorsGame
{
    public static class CombatStatLimits
    {
        public const float MinDamage = 1f;
        public const float MaxDamage = 100f;
        public const float MinKnockback = 0f;
        public const float MaxKnockback = 1000f;
        public const float MinBulletSpeed = 100f;
        public const float MaxBulletSpeed = 1000f;
        public const float MinMeleeSwingSpeed = 0.25f;
        public const float MaxMeleeSwingSpeed = 5f;
    }

    public interface IDamageDealing
    {
        float Damage { get; set; }
        float Knockback { get; set; }
        float CritRate { get; set; }
        float CritDamageMultiplier { get; set; }
    }
}
