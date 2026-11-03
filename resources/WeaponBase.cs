namespace SurvivorsGame
{
    using Godot;
    [GlobalClass]
    public partial class WeaponBase : ItemData, IDamageDealing
    {
        [Export] public PackedScene WeaponVisualScene;

        [ExportGroup("Damage Dealing")]
        [Export] public float DamageBase = 10f;
        public float DamageAdditive;
        public float DamagePercent;

        [Export] public float KnockbackBase = 100f;
        public float KnockbackAdditive;
        public float KnockbackPercent;

        [Export] public float CritRateBase = 2f;
        public float CritRateAdditive;
        public float CritRatePercent;

        [Export] public float CritDamageBase = 1.3f;
        public float CritDamageAdditive;
        public float CritDamagePercent;

        public virtual StatModifier GetStat(string statName) => statName switch
        {
            "Damage" => new StatModifier(DamageBase, DamageAdditive, DamagePercent, CombatStatLimits.MinDamage, CombatStatLimits.MaxDamage),
            "Knockback" => new StatModifier(KnockbackBase, KnockbackAdditive, KnockbackPercent, CombatStatLimits.MinKnockback, CombatStatLimits.MaxKnockback),
            "CritRate" => new StatModifier(CritRateBase, CritRateAdditive, CritRatePercent),
            "CritDamage" => new StatModifier(CritDamageBase, CritDamageAdditive, CritDamagePercent),
            _ => default
        };
    }
}