using Godot;
using System;
using System.IO;

public partial class EnemySpawner : Path2D
{
    [Export] public PackedScene EnemyScene;
    [Export] public double spawnInterval = 0.5;
    private PathFollow2D pathFollow;
    private double spawnTimer;

    public override void _Ready()
    {
        pathFollow = GetNodeOrNull<PathFollow2D>("PathFollow2D");
        if (EnemyScene == null)
        {
            GD.PrintErr("EnemyScene is not assigned in EnemySpawner.");
            return;
        }
        if (pathFollow == null)
        {
            GD.PrintErr("PathFollow2D node not found as a child of EnemySpawner.");
            return;
        }
        if (EnemyScene == null)
        {
            GD.PrintErr("EnemyScene is not assigned in EnemySpawner.");
            return;
        }
        pathFollow.Loop = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        spawnTimer -= delta;
        if (spawnTimer <= 0)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }

    }

    private void SpawnEnemy()
    {
        pathFollow.ProgressRatio = GD.Randf() * 1.0f; // Random progress along the path
        Node2D enemyInstance = EnemyScene.Instantiate() as Node2D;
        GetTree().Root.AddChild(enemyInstance);
        enemyInstance.GlobalPosition = pathFollow.GlobalPosition;
    }


}
