namespace SurvivorsGame
{
    using System.Collections.Generic;
    using System.Text.Json;
    using Godot;
    public partial class SaveSystem : Node
    {
        private string _savePath = "user://savegame.json";
        private string _upgradesPath = "user://upgrades.json";

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true
        };

        public void SaveData(GameSaveData dataToSave)
        {
            SaveToFile(_savePath, dataToSave);
        }

        public GameSaveData LoadData()
        {
            return LoadFromFile<GameSaveData>(_savePath);
        }

        public void SaveUpgrades(UpgradeSaveData upgradesToSave)
        {
            SaveToFile(_upgradesPath, upgradesToSave);
        }

        public UpgradeSaveData LoadUpgrades()
        {
            return LoadFromFile<UpgradeSaveData>(_upgradesPath);
        }

        private void SaveToFile<T>(string path, T data)
        {
            using FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Write);
            if (file == null)
            {
                GD.PrintErr($"SaveSystem: Failed to open file for writing at {path}. Error: {FileAccess.GetOpenError()}");
                return;
            }

            byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(data, _jsonOptions);
            file.StoreBuffer(jsonBytes);
        }

        private T LoadFromFile<T>(string path) where T : new()
        {
            if (!FileAccess.FileExists(path)) return new T();

            using FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
            if (file == null)
            {
                GD.PrintErr($"SaveSystem: Failed to open file for reading at {path}. Error: {FileAccess.GetOpenError()}");
                return new T();
            }

            byte[] jsonBytes = file.GetBuffer((long)file.GetLength());
            if (jsonBytes.Length == 0) return new T();

            try
            {
                return JsonSerializer.Deserialize<T>(jsonBytes, _jsonOptions) ?? new T();
            }
            catch (JsonException ex)
            {
                GD.PrintErr($"SaveSystem: Failed to parse JSON at {path}. Returning default data. Details: {ex.Message}");
                return new T();
            }
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

    public class UpgradeSaveData
    {
        public Dictionary<string, int> PurchasedUpgrades { get; set; } = new();
    }

}