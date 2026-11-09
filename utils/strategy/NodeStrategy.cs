namespace SurvivorsGame
{
    using System.Collections.Generic;
    using Godot;
    using Godot.Collections;

    [Tool]
    [GlobalClass]
    public partial class NodeStrategy : Strategy
    {
        private PackedScene _packedScene;
        private readonly System.Collections.Generic.Dictionary<ulong, List<Node2D>> _sceneInstancesByNode = new();

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
        [Export] public bool UniqueScenePerStack { get; set; }

        public NodeStrategy()
        {
            ItemName = "Add Node";
            ItemDescription = "Adds Node to Upgradable";
            MaxStackSize = 1;
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            if (upgradable is not Node node || _packedScene == null) return;

            if (UniqueScenePerStack)
            {
                ApplyUniqueStackScenes(node);
            }
            else
            {
                ApplySingleScene(node);
            }
        }

        private void ApplyUniqueStackScenes(Node node)
        {
            EnsureSceneInstanceCount(node, Mathf.Max(1, Count));
        }

        private void ApplySingleScene(Node node)
        {
            EnsureSceneInstanceCount(node, 1);
        }

        private void EnsureSceneInstanceCount(Node node, int desiredCount)
        {
            ulong nodeId = node.GetInstanceId();
            if (!_sceneInstancesByNode.TryGetValue(nodeId, out List<Node2D> sceneInstances))
            {
                sceneInstances = new List<Node2D>();
                _sceneInstancesByNode.Add(nodeId, sceneInstances);
            }

            sceneInstances.RemoveAll(instance =>
                !IsInstanceValid(instance) || instance.GetParent() != node);

            while (sceneInstances.Count < desiredCount)
            {
                Node2D sceneInstance = _packedScene.Instantiate<Node2D>();
                foreach (var key in _subsceneData.Keys)
                {
                    string propName = key.AsString();
                    sceneInstance.Set(propName, _subsceneData[propName]);
                }

                node.AddChild(sceneInstance);
                sceneInstances.Add(sceneInstance);
            }
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
