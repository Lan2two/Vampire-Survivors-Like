namespace SurvivorsGame
{
    using Godot;


    public partial class XpOrb : RigidBody2D
    {
        private DetectionComponent detectionComponent;
        private PathfindComponent pathfindComponent;
        public float XPAmount = 1.0f;
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
            BodyEntered += OnBodyEntered;
            detectionComponent.BodyEntered += OnDetectionBodyEntered;
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
                CallDeferred(MethodName.QueueFree);
            }
        }
    }
}