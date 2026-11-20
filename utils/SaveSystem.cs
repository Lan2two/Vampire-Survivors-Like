namespace SurvivorsGame
{
    using System.Collections.Generic;
    using System.Text.Json;
    using Godot;
    public partial class SaveSystem : Node
    {
        private string _savePath = "user://savegame.json";

        public void SaveData(Dictionary<string, object> dataToSave)
        {
            string jsonString = JsonSerializer.Serialize(dataToSave);
            using FileAccess file = FileAccess.Open(_savePath, FileAccess.ModeFlags.Write);

            if (file == null)
            {
                GD.PrintErr($"Failed to open write file: {FileAccess.GetOpenError()}");
                return;
            }

            file.StoreString(jsonString);
        }

        public Dictionary<string, object> LoadGame()
        {
            if (!FileAccess.FileExists(_savePath))
            {
                GD.Print("Save file not found");
                return null;
            }
            using FileAccess file = FileAccess.Open(_savePath, FileAccess.ModeFlags.Read);
            if (file == null)
            {
                GD.Print($"Failed to open read file: {FileAccess.GetOpenError()}");
                return null;
            }
            string jsonstring = file.GetAsText();
            return JsonSerializer.Deserialize<Dictionary<string, object>>(jsonstring);
        }
    }
}