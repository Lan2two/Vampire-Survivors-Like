using Godot;

namespace SurvivorsGame
{
    public partial class Stats : Node
    {
        public class Data
        {
            public float DamageDealt;
            public float DamageTaken;
            public int EnemiesKilled;
        }
        public static Data data = new Data();

        public static void ResetValues()
        {
            data = new Data();
        }

    }
}