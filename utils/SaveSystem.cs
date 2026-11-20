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
        private JavaScriptObject _onWebExitCallback;
        private bool _sceneChangePending;

        public override void _Ready()
        {
            GetTree().AutoAcceptQuit = false;

            if (OS.HasFeature("web"))
            {
                SetupWebExitHooks();
            }
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

        public void ReloadCurrentSceneDeferred()
        {
            if (!BeginSceneChange())
            {
                return;
            }

            CallDeferred(nameof(DeferredReloadCurrentScene));
        }

        public void ChangeSceneToFileDeferred(string scenePath)
        {
            if (!BeginSceneChange())
            {
                return;
            }

            CallDeferred(nameof(DeferredChangeSceneToFile), scenePath);
        }

        private bool BeginSceneChange()
        {
            if (_sceneChangePending)
            {
                return false;
            }

            _sceneChangePending = true;
            GetTree().SceneChanged += OnSceneChanged;
            return true;
        }

        private void OnSceneChanged()
        {
            SceneTree tree = GetTree();
            tree.SceneChanged -= OnSceneChanged;
            _sceneChangePending = false;
            tree.Paused = false;
        }

        private void CancelSceneChange()
        {
            GetTree().SceneChanged -= OnSceneChanged;
            _sceneChangePending = false;
        }

        private void DeferredReloadCurrentScene()
        {
            Node currentScene = GetTree().CurrentScene;
            if (!GodotObject.IsInstanceValid(currentScene) || string.IsNullOrEmpty(currentScene.SceneFilePath))
            {
                CancelSceneChange();
                GD.PrintErr("SaveSystem: Cannot reload the current scene because its scene path is unavailable.");
                return;
            }

            DeferredChangeSceneToFile(currentScene.SceneFilePath);
        }

        private void DeferredChangeSceneToFile(string scenePath)
        {
            SceneTree tree = GetTree();
            PackedScene scene = ResourceLoader.Load<PackedScene>(
                scenePath,
                "",
                ResourceLoader.CacheMode.IgnoreDeep
            );
            if (scene == null)
            {
                CancelSceneChange();
                GD.PrintErr($"SaveSystem: Failed to load scene at {scenePath}.");
                return;
            }

            Error error = tree.ChangeSceneToPacked(scene);
            if (error != Error.Ok)
            {
                CancelSceneChange();
                GD.PrintErr($"SaveSystem: Failed to change to scene {scenePath}. Error: {error}");
            }
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

    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(GameSaveData))]
    [JsonSerializable(typeof(UpgradeSaveData))]
    internal partial class SaveJsonContext : JsonSerializerContext
    {
    }

}