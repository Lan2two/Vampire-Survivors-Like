namespace SurvivorsGame
{
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Text.Json.Serialization.Metadata;
    using Godot;
    public partial class SaveSystem : Node
    {
        private static string _savePath = "user://savegame.json";
        private static string _upgradesPath = "user://upgrades.json";
        private static string _keybindsPath = "user://keybinds.json";
        private JavaScriptObject _onWebExitCallback;
        private bool _sceneChangePending;

        public override void _Ready()
        {
            GetTree().AutoAcceptQuit = false;

            if (OS.HasFeature("web"))
            {
                SetupWebExitHooks();
            }

            LoadAndApplyKeybinds();
        }

        public override void _Notification(int what)
        {
            if (what == NotificationWMCloseRequest)
            {
                Stats.FlushToSave();
                GetTree().Quit();
            }
        }

        private void SetupWebExitHooks()
        {
            _onWebExitCallback = JavaScriptBridge.CreateCallback(
                Callable.From<Godot.Collections.Array>(OnBrowserExit)
            );

            JavaScriptObject window = JavaScriptBridge.GetInterface("window");
            JavaScriptObject document = JavaScriptBridge.GetInterface("document");

            if (window != null)
            {
                window.Call("addEventListener", "beforeunload", _onWebExitCallback);
                window.Call("addEventListener", "pagehide", _onWebExitCallback);
            }

            if (document != null)
            {
                document.Call("addEventListener", "visibilitychange", _onWebExitCallback);
            }
        }

        private void OnBrowserExit(Godot.Collections.Array args)
        {
            Stats.FlushToSave();
        }

        public static void SaveData(GameSaveData dataToSave)
        {
            SaveToFile(_savePath, dataToSave, SaveJsonContext.Default.GameSaveData);
        }

        public static GameSaveData LoadData()
        {
            return LoadFromFile(_savePath, SaveJsonContext.Default.GameSaveData);
        }

        public static void SaveUpgrades(UpgradeSaveData upgradesToSave)
        {
            SaveToFile(_upgradesPath, upgradesToSave, SaveJsonContext.Default.UpgradeSaveData);
        }

        public static UpgradeSaveData LoadUpgrades()
        {
            return LoadFromFile(_upgradesPath, SaveJsonContext.Default.UpgradeSaveData);
        }

        private static void SaveToFile<T>(string path, T data, JsonTypeInfo<T> typeInfo)
        {
            using FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Write);
            if (file == null)
            {
                GD.PrintErr($"SaveSystem: Failed to open file for writing at {path}. Error: {FileAccess.GetOpenError()}");
                return;
            }

            byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(data, typeInfo);
            file.StoreBuffer(jsonBytes);
        }

        private static T LoadFromFile<T>(string path, JsonTypeInfo<T> typeInfo) where T : new()
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
                return JsonSerializer.Deserialize(jsonBytes, typeInfo) ?? new T();
            }
            catch (JsonException ex)
            {
                GD.PrintErr($"SaveSystem: Failed to parse JSON at {path}. Returning default data. Details: {ex.Message}");
                return new T();
            }
        }

        public static void SaveKeybinds()
        {
            var data = new KeybindSaveData();

            foreach (StringName actionName in InputMap.GetActions())
            {
                string action = actionName.ToString();

                if (action.StartsWith("ui_")) continue;

                var events = InputMap.ActionGetEvents(actionName);

                if (events.Count == 0)
                {
                    data.Bindings[action] = new SavedInput();
                }
                else if (events[0] is InputEventKey keyEvent)
                {
                    data.Bindings[action] = new SavedInput
                    {
                        Keycode = (long)keyEvent.Keycode,
                        PhysicalKeycode = (long)keyEvent.PhysicalKeycode
                    };
                }
                else if (events[0] is InputEventMouseButton mouseEvent)
                {
                    data.Bindings[action] = new SavedInput
                    {
                        IsMouse = true,
                        Code = (long)mouseEvent.ButtonIndex
                    };
                }
            }

            SaveToFile(_keybindsPath, data, SaveJsonContext.Default.KeybindSaveData);
        }


        public static void LoadAndApplyKeybinds()
        {
            InputMap.LoadFromProjectSettings();
            bool hasSavedKeybinds = FileAccess.FileExists(_keybindsPath);
            GD.Print($"SaveSystem: Keybind file { (hasSavedKeybinds ? "found" : "not found") } at {_keybindsPath}.");
            if (!hasSavedKeybinds)
            {
                GD.Print("SaveSystem: Using project keybind defaults.");
                return;
            }

            KeybindSaveData data = LoadFromFile(_keybindsPath, SaveJsonContext.Default.KeybindSaveData);
            GD.Print($"SaveSystem: Applying {data.Bindings.Count} saved keybinds.");

            foreach (var kvp in data.Bindings)
            {
                string action = kvp.Key;
                SavedInput saved = kvp.Value;

                if (!InputMap.HasAction(action))
                {
                    GD.PrintErr($"SaveSystem: Skipping saved keybind for unknown action '{action}'.");
                    continue;
                }

                InputEvent newEvent;
                if (saved.IsMouse)
                {
                    long buttonIndex = saved.Code;
                    if (buttonIndex == 0)
                    {
                        GD.Print($"SaveSystem: Skipping invalid mouse binding for '{action}'.");
                        continue;
                    }

                    newEvent = new InputEventMouseButton { ButtonIndex = (MouseButton)buttonIndex };
                    GD.Print($"SaveSystem: Loaded '{action}' as mouse button {buttonIndex}.");
                }
                else
                {
                    long keycode = saved.Keycode;
                    long physicalKeycode = saved.PhysicalKeycode;

                    // Older saves stored either keycode or physical keycode in Code.
                    if (keycode == 0 && physicalKeycode == 0)
                    {
                        keycode = saved.Code;
                    }

                    if (keycode == 0 && physicalKeycode == 0)
                    {
                        InputMap.ActionEraseEvents(action);
                        GD.Print($"SaveSystem: Loaded '{action}' as unassigned.");
                        continue;
                    }

                    newEvent = new InputEventKey
                    {
                        Keycode = (Key)keycode,
                        PhysicalKeycode = (Key)physicalKeycode
                    };
                    GD.Print($"SaveSystem: Loaded '{action}' with keycode {keycode} and physical keycode {physicalKeycode}.");
                }

                InputMap.ActionEraseEvents(action);
                InputMap.ActionAddEvent(action, newEvent);
            }
        }

        public static void ResetKeybinds()
        {
            if (FileAccess.FileExists(_keybindsPath))
            {
                DirAccess.RemoveAbsolute(_keybindsPath);
            }
            InputMap.LoadFromProjectSettings();
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
        public int Coins { get; set; }
        public int TotalCoins { get; set; }
        public Dictionary<string, int> Inventory { get; set; } = new();
    }

    public class UpgradeSaveData
    {
        public Dictionary<string, int> PurchasedUpgrades { get; set; } = new();
    }

    public class KeybindSaveData
    {
        public Dictionary<string, SavedInput> Bindings { get; set; } = new();
    }

    public class SavedInput
    {
        public bool IsMouse { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public long Code { get; set; }

        public long Keycode { get; set; }
        public long PhysicalKeycode { get; set; }
    }

    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(GameSaveData))]
    [JsonSerializable(typeof(UpgradeSaveData))]
    [JsonSerializable(typeof(KeybindSaveData))]
    internal partial class SaveJsonContext : JsonSerializerContext
    {
    }

}