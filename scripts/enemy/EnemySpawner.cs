namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;
    public partial class EnemySpawner : Path2D
    {
        [Export] private Array<EnemyRow> EnemyTable = new();
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
                //GD.Print(pattern);
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
            if (pattern >= EnemyTable.Count || EnemyTable[pattern] == null || EnemyTable[pattern].Enemies.Count == 0)
            {
                return;
            }

            float randomRatio = (float)GD.RandRange(0.0, 1.0);
            int randEnemy = GD.RandRange(0, EnemyTable[pattern].Enemies.Count - 1);
            pathFollow.ProgressRatio = randomRatio;
            EnemyData _targetData = EnemyTable[pattern].Enemies[randEnemy];

            Enemy enemyInstance = _targetData.EnemyScene.Instantiate<Enemy>();
            if (enemyInstance == null)
            {
                GD.PrintErr("EnemySpawner could not instantiate an Enemy.");
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
            enemyInstance.SetStats(_targetData.Health, _targetData.Defense, _targetData.Speed, _targetData.CoinAmount, _targetData.XpAmount, _targetData.Modulate);
            //GD.Print(_targetData.Modulate);
            //GD.Print(enemyInstance.Modulate);
            enemyInstance.GlobalPosition = pathFollow.GlobalPosition;
        }
    }


}
