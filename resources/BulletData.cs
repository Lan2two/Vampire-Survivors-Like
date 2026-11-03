namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletData : Resource, IDamageDealing
    {
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

        [Export] public float SpeedBase = 300f;
        public float SpeedAdditive;
        public float SpeedPercent;

        [ExportGroup("Gun Stats")]
        [Export] public float MaxTravelDistanceBase = 500f;
        public float MaxTravelDistanceAdditive;
        public float MaxTravelDistancePercent;

        [Export] public float PierceCountBase = 0f;
        public float PierceCountAdditive;
        public float PierceCountPercent;

        [Export] public float GunCooldownBase = 1f;
        public float GunCooldownAdditive;
        public float GunCooldownPercent;

        [Export] public bool BouncingBullets { get; set; } = false;

        public StatModifier GetStat(string statName) => statName switch
        {
            "Damage" => new StatModifier(DamageBase, DamageAdditive, DamagePercent, CombatStatLimits.MinDamage, CombatStatLimits.MaxDamage),
            "Knockback" => new StatModifier(KnockbackBase, KnockbackAdditive, KnockbackPercent, CombatStatLimits.MinKnockback, CombatStatLimits.MaxKnockback),
            "CritRate" => new StatModifier(CritRateBase, CritRateAdditive, CritRatePercent),
            "CritDamage" => new StatModifier(CritDamageBase, CritDamageAdditive, CritDamagePercent),
            "Speed" => new StatModifier(SpeedBase, SpeedAdditive, SpeedPercent, CombatStatLimits.MinBulletSpeed, CombatStatLimits.MaxBulletSpeed),
            "MaxTravelDistance" => new StatModifier(MaxTravelDistanceBase, MaxTravelDistanceAdditive, MaxTravelDistancePercent),
            "PierceCount" => new StatModifier(PierceCountBase, PierceCountAdditive, PierceCountPercent, 0f, float.MaxValue),
            "GunCooldown" => new StatModifier(GunCooldownBase, GunCooldownAdditive, GunCooldownPercent, 0.001f, float.MaxValue),
            _ => default
        };
    }
}
