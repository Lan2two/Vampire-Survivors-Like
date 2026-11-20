namespace SurvivorsGame
{
    using System;
    using Godot;
    using Godot.Collections;

    public partial class EnemySpawner : Path2D
    {
        [Export] public Array<PackedScene> EnemySceneArray;
        [Export] public double spawnInterval = 0.5;
        [Export] private int _maxEnemies = 150;

        private PathFollow2D pathFollow;
        private double spawnTimer;

        public override void _Ready()
        {
            pathFollow = GetNodeOrNull<PathFollow2D>("PathFollow2D");
            if (EnemySceneArray.Count == 0)
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

            pathFollow.Loop = true;
        }

        public override void _PhysicsProcess(double delta)
        {
            spawnTimer -= delta;
            if (spawnTimer > 0)
            {
                return;
            }
            if (Enemy.Alive >= _maxEnemies)
            {
                //GD.Print("Cap Reached");
                return;
            }
            spawnTimer = spawnInterval;
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            float randomRatio = (float)GD.RandRange(0.0, 1.0);
            int randEnemy = GD.RandRange(0, EnemySceneArray.Count - 1);
            pathFollow.ProgressRatio = randomRatio;

            Node2D enemyInstance = EnemySceneArray[randEnemy].Instantiate<Node2D>();
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
