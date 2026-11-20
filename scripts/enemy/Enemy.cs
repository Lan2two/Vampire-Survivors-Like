namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class Enemy : RigidBody2D
    {
        public float XpAmount = 1f;
        public AnimatedSprite2D animatedSprite;
        public CollisionShape2D collisionbox;
        public static event Action EnemyDied;
        public static int _enemiesKilled = 0;
        public static int EnemiesKilled
        {
            get => _enemiesKilled;
            set
            {
                if (_enemiesKilled != value)
                {
                    _enemiesKilled = value;
                    EnemyDied?.Invoke();
                }
            }
        }
        public static int Alive = 0;
        public override void _Ready()
        {
            animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
            collisionbox = GetNode<CollisionShape2D>("CollisionShape2D");
            Alive++;
        }

        public override void _ExitTree()
        {
            Alive--;
        }
    }
}
