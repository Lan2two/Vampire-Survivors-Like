namespace SurvivorsGame
{
    using System.Collections.Generic;

    using Godot;
    using Godot.Collections;

    public partial class EnemySpawner : Path2D, IPoolPrewarmer
    {
        [Export] private Array<EnemyRow> EnemyTable = new();
        [Export] private double PatternTime = 90;
        [Export] private bool LoopPattern = false;
        [Export] private double spawnInterval = 1;
        [Export] private int MaxEnemies = 300;
        [Export] private int InitialPoolSizePerScene = 50;

        private PathFollow2D pathFollow;
        private double _spawnTimer;
        private int pattern = 0;
        private double _patternTimer;

        private readonly System.Collections.Generic.Dictionary<PackedScene, ObjectPool<Enemy>> _enemyPools = new();

        public override void _Ready()
        {
            pathFollow = GetNodeOrNull<PathFollow2D>("PathFollow2D");
            if (EnemyTable == null || EnemyTable.Count == 0)
            {
                GD.PrintErr("No EnemyTable is assigned in EnemySpawner.");
                SetPhysicsProcess(false);
                return;
            }

            if (pathFollow == null)
            {
                GD.PrintErr("PathFollow2D node not found as a child of EnemySpawner.");
                SetPhysicsProcess(false);
                return;
            }

            if (spawnInterval <= 0)
            {
                GD.PrintErr("EnemySpawner requires a positive spawn interval.");
                SetPhysicsProcess(false);
                return;
            }

            _patternTimer = PatternTime;
            pathFollow.Loop = true;
            CallDeferred(MethodName.Onready);
        }

        private void Onready()
        {
            Node currentScene = GetTree().CurrentScene;
            if (currentScene == null) return;

            // Pre-warm pools for every unique EnemyScene in EnemyTable
            foreach (EnemyRow row in EnemyTable)
            {
                if (row?.Enemies == null) continue;
                foreach (EnemyData data in row.Enemies)
                {
                    if (data?.EnemyScene != null && !_enemyPools.ContainsKey(data.EnemyScene))
                    {
                        _enemyPools[data.EnemyScene] = new ObjectPool<Enemy>(data.EnemyScene, currentScene, InitialPoolSizePerScene);
                    }
                }
            }
        }

        public override void _ExitTree()
        {
            foreach (var pool in _enemyPools.Values)
            {
                pool.Clear();
            }
            _enemyPools.Clear();
        }

        public override void _PhysicsProcess(double delta)
        {
            _spawnTimer -= delta;
            _patternTimer -= delta;
            if (_patternTimer < 0)
            {
                if (pattern < EnemyTable.Count - 1)
                {
                    pattern++;
                }
                else if (LoopPattern)
                {
                    pattern = 0;
                }
                _patternTimer = PatternTime;
            }
            if (_spawnTimer > 0) return;

            if (Enemy.Alive >= MaxEnemies)
            {
                return;
            }

            _spawnTimer = spawnInterval / ((Stats.data.Seconds + 10) / 10);
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            if (pattern >= EnemyTable.Count || EnemyTable[pattern] == null || EnemyTable[pattern].Enemies.Count == 0)
            {
                return;
            }

            float randomRatio = (float)GD.RandRange(0.0, 1.0);
            int randEnemy = GD.RandRange(0, EnemyTable[pattern].Enemies.Count - 1);
            pathFollow.ProgressRatio = randomRatio;
            EnemyData _targetData = EnemyTable[pattern].Enemies[randEnemy];

            if (_targetData?.EnemyScene == null) return;

            if (!_enemyPools.TryGetValue(_targetData.EnemyScene, out ObjectPool<Enemy> pool))
            {
                Node currentScene = GetTree().CurrentScene;
                if (currentScene == null) return;
                pool = new ObjectPool<Enemy>(_targetData.EnemyScene, currentScene, InitialPoolSizePerScene);
                _enemyPools[_targetData.EnemyScene] = pool;
            }

            Enemy enemyInstance = pool.Get();
            if (enemyInstance == null)
            {
                GD.PrintErr("EnemySpawner could not get an Enemy from the pool.");
                return;
            }

            enemyInstance.GlobalPosition = pathFollow.GlobalPosition;
            enemyInstance.SetStats(_targetData.Health, _targetData.Defense, _targetData.Speed, _targetData.CoinAmount, _targetData.XpAmount, _targetData.Scale, _targetData.Modulate);
            enemyInstance.ReturnToPool = pool.Return;
            enemyInstance.OnSpawn();
        }

        public IEnumerable<(PackedScene scene, int count)> GetPoolRequirements()
        {
            if (EnemyTable == null) yield break;
            foreach (EnemyRow row in EnemyTable)
            {
                if (row?.Enemies == null) continue;
                foreach (EnemyData data in row.Enemies)
                {
                    if (data?.EnemyScene != null)
                        yield return (data.EnemyScene, InitialPoolSizePerScene);
                }
            }
        }


    }
}
