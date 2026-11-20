namespace SurvivorsGame
{
    using System;
    using Godot;

    public static class PlayerStatsRange
    {
        public const float MinMaxHealth = 1f;
        public const float MaxMaxHealth = 1000f;
        public const float MinMoveSpeed = 20f;
        public const float MaxMoveSpeed = 600f;
    }
    public partial class PlayerStatsData : Resource
    {
        private float _maxHealth = 100f;
        private float _moveSpeed = 100f;

        public float MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = Mathf.Clamp(value, PlayerStatsRange.MinMaxHealth, PlayerStatsRange.MaxMaxHealth);
        }
        public float MoveSpeed
        {
            get => _moveSpeed;
            set => _moveSpeed = Mathf.Clamp(value, PlayerStatsRange.MinMoveSpeed, PlayerStatsRange.MaxMoveSpeed);
        }

    }
}