namespace SurvivorsGame
{
    using Godot;
    using System;
    public partial class Stats : Node
    {
        public static event Action EnemyDied;

        public class Data
        {
            public float DamageDealt;
            public float DamageTaken;
            public int _enemiesKilled;
            public int EnemiesKilled
            {
                get => _enemiesKilled;
                set
                {
                    if (_enemiesKilled != value)
                    {
                        _enemiesKilled = value;
                        EnemyDied?.Invoke();
                    }
                }
            }
            public int TotalSeconds;
        }
        public static Data data = new Data();

        public static void ResetValues()
        {
            data = new Data();
        }

    }
}