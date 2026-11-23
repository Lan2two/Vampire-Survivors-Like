namespace SurvivorsGame
{
    using Godot;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public partial class PoolManager : Node
    {
        public static PoolManager Instance { get; private set; }

        private readonly Dictionary<PackedScene, object> _pools = new();

        public override void _Ready()
        {
            Instance = this;
        }

        public ObjectPool<T> GetPool<T>(PackedScene scene, int initialSize = 0, int expandSize = 16) where T : Node, IPoolable
        {
            if (_pools.TryGetValue(scene, out object existing))
            {
                return (ObjectPool<T>)existing;
            }

            var pool = new ObjectPool<T>(scene, this, initialSize, expandSize);
            _pools[scene] = pool;
            return pool;
        }

        public async Task PrewarmAsync(IReadOnlyList<(PackedScene scene, int count)> requirements, Action<float> onProgress, double maxMsPerFrame = 4.0)
        {
            ClearAllPools();

            Dictionary<PackedScene, int> mergedCounts = new();
            int totalObjects = 0;

            foreach (var (scene, count) in requirements)
            {
                if (scene == null || count <= 0) continue;
                mergedCounts.TryGetValue(scene, out int current);
                if (count > current)
                {
                    totalObjects += count - current;
                    mergedCounts[scene] = count;
                }
            }

            if (totalObjects == 0)
            {
                onProgress?.Invoke(1f);
                return;
            }

            int createdSoFar = 0;
            ulong budgetUsec = (ulong)(maxMsPerFrame * 1000.0);
            ulong frameStartUsec = Time.GetTicksUsec();

            foreach (var kvp in mergedCounts)
            {
                PackedScene scene = kvp.Key;
                int targetCount = kvp.Value;

                Node firstInstance = scene.Instantiate();
                if (firstInstance is not IPoolable)
                {
                    firstInstance.QueueFree();
                    GD.PrintErr($"PoolManager: Scene '{scene.ResourcePath}' root does not implement IPoolable.");
                    continue;
                }

                var pool = GetOrCreateUntypedPool(scene, firstInstance);
                AddChild(firstInstance);
                pool.returnAction(firstInstance);
                createdSoFar++;

                if (Time.GetTicksUsec() - frameStartUsec >= budgetUsec)
                {
                    onProgress?.Invoke((float)createdSoFar / totalObjects);
                    await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
                    frameStartUsec = Time.GetTicksUsec();
                }

                for (int i = 1; i < targetCount; i++)
                {
                    Node instance = scene.Instantiate();
                    AddChild(instance);
                    pool.returnAction(instance);
                    createdSoFar++;

                    if (Time.GetTicksUsec() - frameStartUsec >= budgetUsec)
                    {
                        onProgress?.Invoke((float)createdSoFar / totalObjects);
                        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
                        frameStartUsec = Time.GetTicksUsec();
                    }
                }
            }

            onProgress?.Invoke(1f);
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }

        private (object pool, Action<Node> returnAction) GetOrCreateUntypedPool(PackedScene scene, Node sample)
        {
            if (sample is Bullet)
            {
                var p = GetPool<Bullet>(scene, 0);
                return (p, n => p.Return((Bullet)n));
            }
            if (sample is Enemy)
            {
                var p = GetPool<Enemy>(scene, 0);
                return (p, n => p.Return((Enemy)n));
            }
            if (sample is Absorbable)
            {
                var p = GetPool<Absorbable>(scene, 0);
                return (p, n => p.Return((Absorbable)n));
            }

            throw new InvalidOperationException($"Unsupported IPoolable type: {sample.GetType().Name}");
        }

        public void ClearAllPools()
        {
            foreach (Node child in GetChildren())
            {
                child.QueueFree();
            }
            _pools.Clear();
        }
    }
}
