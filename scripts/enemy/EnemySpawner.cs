namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    public partial class EnemySpawner : Path2D
    {
        [Export] private Array<Array<PackedScene>> EnemyTable;
        [Export] private double PatternTime = 90;
        [Export] private bool LoopPattern = false;
        [Export] private double spawnInterval = 1;
        [Export] private int MaxEnemies = 300;

        private PathFollow2D pathFollow;
        private double _spawnTimer;
        private int pattern = 0;
        private double _patternTimer;

        public override void _Ready()
        {
            pathFollow = GetNodeOrNull<PathFollow2D>("PathFollow2D");
            if (EnemyTable.Count == 0)
            {
                GD.PrintErr("No EnemyScene is not assigned in EnemySpawner.");
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
        }

        public override void _PhysicsProcess(double delta)
        {
            _spawnTimer -= delta;
            _patternTimer -= delta;
            if (_patternTimer < 0)
            {
                if (pattern < EnemyTable.Count)
                {
                    pattern++;
                }
                if (LoopPattern && pattern == EnemyTable.Count)
                {
                    pattern = 0;
                }
                GD.Print(pattern);
                _patternTimer = PatternTime;
            }
            if (_spawnTimer > 0) return;

            if (Enemy.Alive >= MaxEnemies)
            {
                //GD.Print("Cap Reached");
                return;
            }
            _spawnTimer = spawnInterval / ((Stats.data.Seconds + 10) / 10);
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            float randomRatio = (float)GD.RandRange(0.0, 1.0);
            int randEnemy = GD.RandRange(0, EnemyTable[pattern].Count - 1);
            pathFollow.ProgressRatio = randomRatio;

            Node2D enemyInstance = EnemyTable[pattern][randEnemy].Instantiate<Node2D>();
            if (enemyInstance == null)
            {
                GD.PrintErr("EnemySpawner could not instantiate a Node2D enemy.");
                return;
            }

            Node currentScene = GetTree().CurrentScene;
            if (currentScene == null)
            {
                enemyInstance.Free();
                GD.PrintErr("EnemySpawner cannot spawn because there is no current scene.");
                return;
            }

            currentScene.AddChild(enemyInstance);
            enemyInstance.GlobalPosition = pathFollow.GlobalPosition;
        }
    }
}
