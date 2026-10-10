using Godot;
using System;

public partial class Game : Node2D
{
    Label gameOverLabel;
    CanvasLayer GUI;

    private bool _isChangingScene = false;
    public override void _Ready()
    {
        Enemy.EnemyDied += CheckforGameOver;
        GUI = GetNodeOrNull<CanvasLayer>("GUI");
        gameOverLabel = GUI.GetNodeOrNull<Label>("GameOver");
    }

    private void CheckforGameOver(Enemy enemy)
    {
        if (_isChangingScene || !IsInstanceValid(this)) return;
        if (Enemy.EnemiesKilled >= 20)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        if (_isChangingScene || !IsInstanceValid(this)) return;
        GetTree().Paused = true;
        gameOverLabel.Visible = true;
        Enemy.EnemiesKilled = 0;
        GetTree().CreateTimer(3).Timeout += ReturnToMenu;
    }

    private void ReturnToMenu()
    {
        GetTree().Paused = false;
        CallDeferred(nameof(DeferredChangeScene));
    }

    private void DeferredChangeScene()
    {
        if (IsInstanceValid(this) && IsInsideTree())
        {
            GetTree().ChangeSceneToFile("res://scenes/start_menu.tscn");
        }
    }
}
