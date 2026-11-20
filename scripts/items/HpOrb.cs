namespace SurvivorsGame
{
    using Godot;
    public partial class HpOrb : RigidBody2D
    {
        private DetectionComponent detectionComponent;
        private PathfindComponent pathfindComponent;
        public float HPAmount = 10f;
        public bool SawPlayer = false;
        public override void _Ready()
        {
            detectionComponent = GetNodeOrNull<DetectionComponent>("DetectionComponent");
            pathfindComponent = GetNodeOrNull<PathfindComponent>("PathfindComponent");
            if (detectionComponent == null)
            {
                GD.PrintErr("DetectionComponent node not found in XpOrb");
                return;
            }
            if (pathfindComponent == null)
            {
                GD.PrintErr("PathfindComponent not found in XpOrb");
                return;
            }
            BodyEntered += OnBodyEntered;
            detectionComponent.BodyEntered += OnDetectionBodyEntered;
        }

        public override void _ExitTree()
        {
            BodyEntered -= OnBodyEntered;
            if (detectionComponent != null)
            {
                detectionComponent.BodyEntered -= OnDetectionBodyEntered;
            }
        }


        private void OnDetectionBodyEntered(Node body)
        {
            if (body is Player)
            {
                SawPlayer = true;
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            if (SawPlayer)
            {
                pathfindComponent.PathfindToPlayer();
            }
        }

        private void OnBodyEntered(Node body)
        {
            if (body is Player)
            {
                Player player = (Player)body;
                player.healthComponent.currentHealth += HPAmount;
                CallDeferred(Node.MethodName.QueueFree);
            }
        }
    }
}