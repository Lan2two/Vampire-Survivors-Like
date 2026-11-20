namespace SurvivorsGame
{
    using Godot;

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
        public const float MinGunCooldown = 0.05f;
        public const float MinSpread = 0f;
    }

    public enum UpgradeCalculationMode { Additive, Percentage }

    public interface IDamageDealing
    {
        StatModifier GetStat(string statName);
    }

    public readonly struct StatModifier(float @base, float additive, float percent, float min = float.MinValue, float max = float.MaxValue)
    {
        public readonly float Base = @base;
        public readonly float Additive = additive;
        public readonly float Percent = percent;
        public readonly float Min = min;
        public readonly float Max = max;

        public float CalculateFinal() => Mathf.Clamp((Base + Additive) * (1 + (Percent / 100)), Min, Max);
    }
}
