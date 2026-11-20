namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    [Tool] // Required to process inspector rendering in-editor
    [GlobalClass]
    public partial class PlayerNodeStrategy : Strategy
    {
        private PackedScene _packedScene;
        private bool called = false;

        // Fully serialized dictionary tracking dynamic configurations
        [Export] private Dictionary _subsceneData = new();

        [Export]
        public PackedScene PackedScene
        {
            get => _packedScene;
            set
            {
                if (_packedScene != value)
                {
                    _packedScene = value;
                    _subsceneData.Clear(); // Flush outdated settings
                    NotifyPropertyListChanged(); // Triggers real-time inspector refresh
                }
            }
        }

        public PlayerNodeStrategy()
        {
            upgradeType = UpgradeType.Player;
            ItemName = "Add Node";
            ItemDescription = "Adds Node to Player";
            MaxStackSize = 1;
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            if (upgradable is not Node || called == true)
            {
                return;
            }
            Node node = upgradable as Node;
            if (_packedScene == null || node == null)
            {
                return;
            }

            Node2D sceneInstance = _packedScene.Instantiate<Node2D>();

            // Reapply stored values safely via Godot's reflection API
            foreach (var key in _subsceneData.Keys)
            {
                string propName = key.AsString();
                sceneInstance.Set(propName, _subsceneData[propName]);
            }

            node.AddChild(sceneInstance);
            called = true;
        }


        // 1. Tell Godot's inspector what properties to display dynamically
        public override Array<Dictionary> _GetPropertyList()
        {
            var properties = new Array<Dictionary>();

            if (_packedScene == null) return properties;

            // Instantiate a dummy instance in-editor memory to grab its script exports
            Node tempInstance = _packedScene.Instantiate();
            var script = tempInstance.GetScript().As<Script>();

            if (script != null)
            {
                foreach (Dictionary prop in script.GetScriptPropertyList())
                {
                    long usage = prop["usage"].AsInt64();

                    // Filter exclusively for fields decorated with [Export]
                    if ((usage & (long)PropertyUsageFlags.Editor) != 0)
                    {
                        string propName = prop["name"].AsString();

                        // Formats beautifully as a distinct expandable Category in your inspector
                        properties.Add(new Dictionary
                        {
                            { "name", $"Subscene Configuration/{propName}" },
                            { "type", prop["type"] },
                            { "usage", (long)PropertyUsageFlags.Editor | (long)PropertyUsageFlags.Storage }
                        });

                        // Populate defaults from the node if not altered yet
                        if (!_subsceneData.ContainsKey(propName))
                        {
                            _subsceneData[propName] = tempInstance.Get(propName);
                        }
                    }
                }
            }

            tempInstance.Free(); // Explicit garbage collection in-editor
            return properties;
        }

        // 2. Fetch value from the data map when Godot requests it for the inspector UI
        public override Variant _Get(StringName property)
        {
            string propStr = property.ToString();
            if (propStr.StartsWith("Subscene Configuration/"))
            {
                string actualKey = propStr.Replace("Subscene Configuration/", "");
                return _subsceneData.ContainsKey(actualKey) ? _subsceneData[actualKey] : default;
            }
            return base._Get(property);
        }

        // 3. Store edited values directly back to your Strategy file
        public override bool _Set(StringName property, Variant value)
        {
            string propStr = property.ToString();
            if (propStr.StartsWith("Subscene Configuration/"))
            {
                string actualKey = propStr.Replace("Subscene Configuration/", "");
                _subsceneData[actualKey] = value;
                return true;
            }
            return base._Set(property, value);
        }
    }
}