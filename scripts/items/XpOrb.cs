namespace SurvivorsGame
{
    using Godot;
    public partial class XpOrb : RigidBody2D
    {
        private DetectionComponent detectionComponent;
        private PathfindComponent pathfindComponent;
        public float XPAmount = 1.0f;
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
        }

        public override void _ExitTree()
        {
            BodyEntered -= OnBodyEntered;
        }

        public override void _PhysicsProcess(double delta)
        {
            if (detectionComponent.IsPlayerInRange())
            {
                pathfindComponent.PathfindToPlayer();
            }
        }


        public void SetXPAmount(float amount)
        {
            XPAmount = amount;
        }

        private void OnBodyEntered(Node body)
        {
            if (body is Player)
            {
                Player player = (Player)body;
                player.playerXP += XPAmount;
                player.totalXP += XPAmount;
                CallDeferred(Node.MethodName.QueueFree);
            }
        }
    }
}