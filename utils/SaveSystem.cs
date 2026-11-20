namespace SurvivorsGame
{
    using System.Collections.Generic;
    using System.Text.Json;
    using Godot;
    public partial class SaveSystem : Node
    {
        private string _savePath = "user://savegame.json";

        public void SaveData(GameSaveData dataToSave)
        {
            using FileAccess file = FileAccess.Open(_savePath, FileAccess.ModeFlags.Write);
            if (file == null) return;
            byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(dataToSave);
            file.StoreBuffer(jsonBytes);
        }

        public GameSaveData LoadData()
        {
            if (!FileAccess.FileExists(_savePath)) return new GameSaveData();

            using FileAccess file = FileAccess.Open(_savePath, FileAccess.ModeFlags.Read);
            if (file == null) return new GameSaveData();

            byte[] jsonBytes = file.GetBuffer((long)file.GetLength());
            return JsonSerializer.Deserialize<GameSaveData>(jsonBytes);
        }

    }

    public class GameSaveData
    {
        public int TotalSeconds { get; set; }
        public int TotalKills { get; set; }
        public int TotalDamageDealt { get; set; }
        public int TotalDamageTaken { get; set; }
        public int TotalHealthHealed { get; set; }
        public int TotalCampfiresLit { get; set; }
        public int TotalTotalXp { get; set; }
        public int TotalCoins { get; set; }
        public Dictionary<string, int> Inventory { get; set; } = new();
    }

}