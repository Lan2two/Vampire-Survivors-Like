using Godot;
using System;

public partial class KillsCounter : Label
{
    public override void _Ready()
    {
        Text = $"Kills: {Enemy.EnemiesKilled}";
        Enemy.EnemyDied += UpdateCounter;
    }

    public override void _ExitTree()
    {
        Enemy.EnemyDied -= UpdateCounter;
    }

    private void UpdateCounter(Enemy enemy)
    {
        Text = $"Kills: {Enemy.EnemiesKilled}";
    }


}
