namespace SurvivorsGame
{
    using Godot;
    using System;

    [GlobalClass]
    public partial class DetectionComponent : Area2D
    {
        public CollisionShape2D shape;
        private bool playerInRange = false;
        public override void _Ready()
        {
            shape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
            BodyEntered += OnBodyEntered;
            BodyExited += OnBodyExit;
        }

        public override void _ExitTree()
        {
            BodyEntered -= OnBodyEntered;
            BodyExited -= OnBodyExit;
        }

        public bool IsPlayerInRange()
        {
            return playerInRange;
        }

        private void OnBodyEntered(Node body)
        {
            if (body is Player)
            {
                playerInRange = true;
            }
        }

        private void OnBodyExit(Node body)
        {
            if (body is Player)
            {
                playerInRange = false;
            }
        }
    }
}
