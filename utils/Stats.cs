namespace SurvivorsGame
{
    using Godot;
    using System;
    public partial class Stats : Node
    {
        public static event Action EnemyDied;

        public class Data
        {
            public int Coins;
            public float DamageDealt;
            public float DamageTaken;
            public float HealthHealed;
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
            public int Seconds;
            public int CampfiresLit;
            public int Level;
            public float totalXP;
        }
        public static Data data = new Data();
        public static Data _savedData = new Data();

        public static void ResetValues()
        {
            data = new Data();
            _savedData = new Data();
        }

        public static void FlushToSave()
        {
            int deltaSeconds = data.Seconds - _savedData.Seconds;
            int deltaKills = data.EnemiesKilled - _savedData.EnemiesKilled;
            int deltaDamageDealt = (int)data.DamageDealt - (int)_savedData.DamageDealt;
            int deltaDamageTaken = (int)data.DamageTaken - (int)_savedData.DamageTaken;
            int deltaHealthHealed = (int)data.HealthHealed - (int)_savedData.HealthHealed;
            int deltaCampfiresLit = data.CampfiresLit - _savedData.CampfiresLit;
            int deltaTotalXp = (int)data.totalXP - (int)_savedData.totalXP;
            int deltaCoins = data.Coins - _savedData.Coins;

            if (deltaSeconds <= 0 && deltaKills <= 0 && deltaDamageDealt <= 0 &&
                deltaDamageTaken <= 0 && deltaHealthHealed <= 0 && deltaCampfiresLit <= 0 &&
                deltaTotalXp <= 0 && deltaCoins <= 0)
            {
                return;
            }

            GameSaveData save = SaveSystem.Load<GameSaveData>();
            save.TotalSeconds += deltaSeconds;
            save.TotalKills += deltaKills;
            save.TotalDamageDealt += deltaDamageDealt;
            save.TotalDamageTaken += deltaDamageTaken;
            save.TotalHealthHealed += deltaHealthHealed;
            save.TotalCampfiresLit += deltaCampfiresLit;
            save.TotalTotalXp += deltaTotalXp;
            save.Coins += deltaCoins;
            save.TotalCoins += deltaCoins;
            SaveSystem.Save(save);

            _savedData.Seconds += deltaSeconds;
            _savedData.EnemiesKilled += deltaKills;
            _savedData.DamageDealt += deltaDamageDealt;
            _savedData.DamageTaken += deltaDamageTaken;
            _savedData.HealthHealed += deltaHealthHealed;
            _savedData.CampfiresLit += deltaCampfiresLit;
            _savedData.totalXP += deltaTotalXp;
            _savedData.Coins += deltaCoins;
        }

    }
}