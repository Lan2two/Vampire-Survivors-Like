namespace SurvivorsGame
{
    using System.Collections.Generic;

    using Godot;
    using Godot.Collections;

    public partial class LootComponent : Node, IPoolPrewarmer
    {
        [Export] Array<PackedScene> LootTable = new();
        [Export] float XpDropRate = 100;
        [Export] float HealthDropRate = .1f;
        [Export] float CoinDroprate = 1f;
        [Export] float LootDropRate = .05f;
        [Export] Enemy enemy;
        [Export] EnemyDeath deathState;
        [Export] PackedScene XpOrbScene;
        [Export] PackedScene HpOrbScene;
        [Export] PackedScene CoinScene;

        private const int InitialXpPool = 300;
        private const int InitialHpPool = 30;
        private const int InitialCoinPool = 30;

        private Node currentScene;
        private static Node _cachedPoolScene;
        private static readonly System.Collections.Generic.Dictionary<PackedScene, ObjectPool<Absorbable>> _sharedPools = new();

        public override void _Ready()
        {
            if (deathState == null)
            {
                GD.PrintErr("EnemyDeath unassigned in LootComponent.");
                return;
            }
            if (enemy == null)
            {
                GD.PrintErr("Enemy unassigned in LootComponent.");
                return;
            }
            if (XpOrbScene == null)
            {
                GD.PrintErr("XpOrbScene unassigned in LootComponent.");
                return;
            }

            currentScene = GetTree().CurrentScene;
            deathState.OnDeath += _OnDeath;
            CallDeferred(MethodName.PrewarmPools);
        }

        private void PrewarmPools()
        {
            if (XpOrbScene != null) GetOrCreatePool(XpOrbScene, InitialXpPool);
            if (HpOrbScene != null) GetOrCreatePool(HpOrbScene, InitialHpPool);
            if (CoinScene != null) GetOrCreatePool(CoinScene, InitialCoinPool);
        }

        public override void _ExitTree()
        {
            if (deathState != null)
            {
                deathState.OnDeath -= _OnDeath;
            }
        }

        private ObjectPool<Absorbable> GetOrCreatePool(PackedScene scene, int initialSize = 100)
        {
            if (!IsInstanceValid(currentScene))
            {
                currentScene = GetTree().CurrentScene;
            }
            if (currentScene == null)
            {
                return null;
            }
            if (_cachedPoolScene != currentScene)
            {
                _sharedPools.Clear();
                _cachedPoolScene = currentScene;
            }

            if (!_sharedPools.TryGetValue(scene, out ObjectPool<Absorbable> pool))
            {
                pool = new ObjectPool<Absorbable>(scene, currentScene, initialSize);
                _sharedPools[scene] = pool;
            }

            return pool;
        }

        private void _OnDeath()
        {
            DropAbsorbable(XpOrbScene, XpDropRate, enemy.XpAmount);
            DropAbsorbable(HpOrbScene, HealthDropRate);
            DropAbsorbable(CoinScene, CoinDroprate, enemy.CoinAmount);
            DropItem(LootDropRate);
        }

        public void DropAbsorbable(PackedScene packedScene, float chance, float amount = 0)
        {
            if (packedScene == null)
            {
                GD.PrintErr("Dropped scene is null");
                return;
            }

            float rand = (float)GD.RandRange(0f, 100f);
            if (rand > chance)
            {
                return;
            }

            ObjectPool<Absorbable> pool = GetOrCreatePool(packedScene);
            if (pool == null)
            {
                GD.PrintErr("LootComponent cannot drop loot because there is no current scene.");
                return;
            }

            Absorbable instance = pool.Get();
            if (instance == null)
            {
                GD.PrintErr("LootComponent failed to get Absorbable from pool.");
                return;
            }

            instance.GlobalPosition = enemy.GlobalPosition + new Vector2(GD.RandRange(-5, 5), GD.RandRange(-5, 5));
            instance.SetAmount(amount);
            instance.ReturnToPool = pool.Return;
            instance.OnSpawn();
        }

        public void DropItem(float chance)
        {
            if (LootTable.Count == 0)
            {
                return;
            }

            float lootChance = (float)GD.RandRange(0f, 100f);
            if (lootChance > chance)
            {
                return;
            }

            int randItemIndex = GD.RandRange(0, LootTable.Count - 1);
            DropScene(LootTable[randItemIndex]);
        }

        private Node2D DropScene(PackedScene scene)
        {
            Node2D sceneInstance = scene.Instantiate<Node2D>();
            if (sceneInstance == null)
            {
                GD.PrintErr("LootComponent failed to instantiate Scene.");
                return null;
            }

            if (!IsInstanceValid(currentScene))
            {
                currentScene = GetTree().CurrentScene;
            }
            if (currentScene == null)
            {
                sceneInstance.Free();
                GD.PrintErr("LootComponent cannot drop loot because there is no current scene.");
                return null;
            }

            sceneInstance.GlobalPosition = enemy.GlobalPosition + new Vector2(GD.RandRange(-5, 5), GD.RandRange(-5, 5));
            currentScene.CallDeferred(Node.MethodName.AddChild, sceneInstance);
            return sceneInstance;
        }

        public IEnumerable<(PackedScene scene, int count)> GetPoolRequirements()
        {
            if (XpOrbScene != null) yield return (XpOrbScene, InitialXpPool);
            if (HpOrbScene != null) yield return (HpOrbScene, InitialHpPool);
            if (CoinScene != null) yield return (CoinScene, InitialCoinPool);
        }

    }
}
