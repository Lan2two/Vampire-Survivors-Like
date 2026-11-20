namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class ProjectileData : Resource, IDamageDealing
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

        [Export] public float CritDamageBase = 30f;
        public float CritDamageAdditive;
        public float CritDamagePercent;

        [Export] public float SpeedBase = 300f;
        public float SpeedAdditive;
        public float SpeedPercent;

        [ExportGroup("Projectile Stats")]
        [Export] public float MaxTravelDistanceBase = 500f;
        public float MaxTravelDistanceAdditive;
        public float MaxTravelDistancePercent;

        [Export] public float PierceCountBase = 0f;
        public float PierceCountAdditive;
        public float PierceCountPercent;

        [Export] public float ProjectileCooldownBase = 1f;
        public float ProjectileCooldownAdditive;
        public float ProjectileCooldownPercent;

        [Export] public int BurstBase = 1;
        public float BurstAdditive;
        public float BurstPercent;

        [Export] public float BurstIntervalBase = .2f;
        public float BurstIntervalAdditive;
        public float BurstIntervalPercent;

        [Export] public float SpreadBase = 0f;
        public float SpreadAdditive;
        public float SpreadPercent;

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
            "ProjectileCooldown" => new StatModifier(ProjectileCooldownBase, ProjectileCooldownAdditive, ProjectileCooldownPercent, CombatStatLimits.MinProjectileCooldown, float.MaxValue),
            "Burst" => new StatModifier(BurstBase, BurstAdditive, BurstPercent, 0, float.MaxValue),
            "BurstInterval" => new StatModifier(BurstIntervalBase, BurstIntervalAdditive, BurstIntervalPercent, CombatStatLimits.MinProjectileCooldown, float.MaxValue),
            "Spread" => new StatModifier(SpreadBase, SpreadAdditive, SpreadPercent, CombatStatLimits.MinSpread, float.MaxValue),
            _ => default
        };
    }
}
