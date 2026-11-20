namespace SurvivorsGame
{
    using Godot;


    public partial class XpOrb : RigidBody2D
    {
        private DetectionComponent detectionComponent;
        private PathfindComponent pathfindComponent;
        public float XPAmount = 1.0f;
        private bool SawPlayer = false;
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
                GD.Print("Saw Player");
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            if (SawPlayer)
            {
                GD.Print("pathfinding");
                pathfindComponent.PathfindToPlayer();
            }
        }


        public void SetXPAmount(float amount)
        {
            GD.Print("Set xp");
            XPAmount = amount;
        }


        private void OnBodyEntered(Node body)
        {
            GD.Print("On Body Entered");
            if (body is Player)
            {
                GD.Print("check if body is player");
                Player player = (Player)body;
                GD.Print("set body to player");
                player.playerXP += XPAmount;
                GD.Print($"Player collected {(int)XPAmount} XP. Total XP: {(int)player.playerXP}");
                GD.Print("Freeing queue");
                CallDeferred(MethodName.QueueFree);
            }
        }
    }
}