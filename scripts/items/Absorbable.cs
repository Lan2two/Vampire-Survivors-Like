namespace SurvivorsGame
{
    using Godot;
    [GlobalClass]
    public partial class Absorbable : RigidBody2D
    {
        [Export] private DetectionComponent detectionComponent;
        [Export] private PathfindComponent pathfindComponent;
        public bool SawPlayer;
        public override void _Ready()
        {
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
            if (detectionComponent.IsPlayerInRange() || SawPlayer)
            {
                pathfindComponent.PathfindToPlayer();
            }
        }

        private void OnBodyEntered(Node body)
        {
            if (body is Player)
            {
                CallDeferred(Node.MethodName.QueueFree);
            }
        }
    }
}