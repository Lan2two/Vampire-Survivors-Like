using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    public AnimatedSprite2D anim;
    public CollisionShape2D collisionbox;
    public static event Action<Enemy> EnemyDied;
    public static int _enemiesKilled = 0;
    public static int EnemiesKilled
    {
        get => _enemiesKilled;
        set
        {
            if (_enemiesKilled != value)
            {
                _enemiesKilled = value;
                EnemyDied?.Invoke(null);
            }
        }
    }
    public override void _Ready()
    {
        anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        collisionbox = GetNode<CollisionShape2D>("CollisionShape2D");
    }
}
