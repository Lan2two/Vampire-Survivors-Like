namespace SurvivorsGame
{
    using Godot;

    public static class AbsorbableStatsRange
    {
        public const float MinRadius = 5f;
        public const float MaxRadius = 500f;
        public const float MinMoveSpeed = 100f;
        public const float MaxMoveSpeed = 600f;
    }

    [GlobalClass]
    public partial class AbsorbableData : Resource
    {
        [ExportGroup("Absorbable Attributes")]
        [Export] public float RadiusBase = 42f;
        public float RadiusAdditive;
        public float RadiusPercent;

        [Export] public float MoveSpeedBase = 100f;
        public float MoveSpeedAdditive;
        public float MoveSpeedPercent;

        public StatModifier GetStat(string statName) => statName switch
        {
            "Radius" => new StatModifier(RadiusBase, RadiusAdditive, RadiusPercent, AbsorbableStatsRange.MinRadius, AbsorbableStatsRange.MaxRadius),
            "MoveSpeed" => new StatModifier(MoveSpeedBase, MoveSpeedAdditive, MoveSpeedPercent, AbsorbableStatsRange.MinMoveSpeed, AbsorbableStatsRange.MaxMoveSpeed),
            _ => default
        };
    }
}
