namespace SurvivorsGame
{
    using Godot;

    public static class PlayerStatsRange
    {
        public const float MinMaxHealth = 1f;
        public const float MaxMaxHealth = 1000f;
        public const float MinMoveSpeed = 20f;
        public const float MaxMoveSpeed = 600f;
    }

    [GlobalClass]
    public partial class PlayerStatsData : Resource
    {
        [ExportGroup("Player Attributes")]
        [Export] public float MaxHealthBase = 100f;
        public float MaxHealthAdditive;
        public float MaxHealthPercent;

        [Export] public float MoveSpeedBase = 100f;
        public float MoveSpeedAdditive;
        public float MoveSpeedPercent;

        // Custom factory method specifically for handling player stat blocks
        public StatModifier GetStat(string statName) => statName switch
        {
            "MaxHealth" => new StatModifier(MaxHealthBase, MaxHealthAdditive, MaxHealthPercent, PlayerStatsRange.MinMaxHealth, PlayerStatsRange.MaxMaxHealth),
            "MoveSpeed" => new StatModifier(MoveSpeedBase, MoveSpeedAdditive, MoveSpeedPercent, PlayerStatsRange.MinMoveSpeed, PlayerStatsRange.MaxMoveSpeed),
            _ => default
        };
    }
}
