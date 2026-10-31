namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    [Tool]
    [GlobalClass]
    public partial class NodeStrategy : Strategy
    {
        private PackedScene _packedScene;
        Node2D sceneInstance;
        Node currentNode;

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
                    _subsceneData.Clear();

                    // Only notify the inspector if we are running inside the editor toolset
#if TOOLS
                    NotifyPropertyListChanged();
#endif
                }
            }
        }
        [Export] public override UpgradeType upgradeType { get; set; }

        public NodeStrategy()
        {
            ItemName = "Add Node";
            ItemDescription = "Adds Node to Upgradable";
            MaxStackSize = 1;
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            if (upgradable is not Node) return;

            Node node = upgradable as Node;
            if (_packedScene == null || node == null) return;

            if (sceneInstance == null || currentNode != node)
            {
                sceneInstance = _packedScene.Instantiate<Node2D>();
                currentNode = node;
            }
            if (node.GetChildren().Contains(sceneInstance)) return;

            // Reapply stored values safely via Godot's reflection API
            foreach (var key in _subsceneData.Keys)
            {
                string propName = key.AsString();
                sceneInstance.Set(propName, _subsceneData[propName]);
            }
            node.AddChild(sceneInstance);
        }

        // ==========================================
        // EDITOR ONLY LOGIC
        // Completely hidden from Web/Release exports
        // ==========================================
#if TOOLS
        public override Array<Dictionary> _GetPropertyList()
        {
            var properties = new Array<Dictionary>();

            if (_packedScene == null) return properties;

            Node tempInstance = _packedScene.Instantiate();
            if (tempInstance == null) return properties;

            var script = tempInstance.GetScript().As<Script>();

            if (script != null)
            {
                foreach (Dictionary prop in script.GetScriptPropertyList())
                {
                    long usage = prop["usage"].AsInt64();

                    if ((usage & (long)PropertyUsageFlags.Editor) != 0)
                    {
                        string propName = prop["name"].AsString();

                        properties.Add(new Dictionary
                        {
                            { "name", $"Subscene Configuration/{propName}" },
                            { "type", prop["type"] },
                            { "usage", (long)PropertyUsageFlags.Editor | (long)PropertyUsageFlags.Storage }
                        });

                        if (!_subsceneData.ContainsKey(propName))
                        {
                            _subsceneData[propName] = tempInstance.Get(propName);
                        }
                    }
                }
            }

            tempInstance.Free();
            return properties;
        }

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
#endif
    }
}
