namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class MeleeWeaponBase : WeaponBase
    {
        [ExportGroup("Melee Settings")]
        [Export] public float SwingSpeedBase = 1.0f;
        public float SwingSpeedAdditive;
        public float SwingSpeedPercent;

        [Export] public float SwingCooldownBase = 1.0f;
        public float SwingCooldownAdditive;
        public float SwingCooldownPercent;

        // Extending the factory method allows us to seamlessly append our unique melee attributes
        public override StatModifier GetStat(string statName) => statName switch
        {
            "SwingSpeed" => new StatModifier(SwingSpeedBase, SwingSpeedAdditive, SwingSpeedPercent, CombatStatLimits.MinMeleeSwingSpeed, CombatStatLimits.MaxMeleeSwingSpeed),
            "SwingCooldown" => new StatModifier(SwingCooldownBase, SwingCooldownAdditive, SwingCooldownPercent, 0.001f, float.MaxValue),
            _ => base.GetStat(statName) // Gracefully fall back to parent WeaponBase fields (Damage, Knockback, Crit)
        };
    }
}
