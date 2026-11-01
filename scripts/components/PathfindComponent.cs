using Godot;

namespace SurvivorsGame
{
    [GlobalClass]
    public partial class PathfindComponent : NavigationAgent2D
    {
        [Export] private RigidBody2D characterBody;
        [Export] private VelocityComponent velocityComponent;
        [Export] private float threshold = 3.0f;
        [Export] private float pathUpdateInterval = 0.2f;

        private float distanceToTarget;
        private bool targetReached = false;
        private bool hasTargetPosition;
        private Player player;
        private float timeSinceLastUpdate = 0.0f;

        public override void _Ready()
        {
            player = this.GetPlayer();
            TargetDesiredDistance = threshold;
            AvoidanceEnabled = false;

            if (characterBody == null || velocityComponent == null)
            {
                GD.PrintErr("Missing dependencies in PathfindComponent");
                return;
            }

            timeSinceLastUpdate = (float)GD.RandRange(0.0f, pathUpdateInterval);
        }

        public void PathfindToPlayer()
        {
            if (characterBody == null || velocityComponent == null || player == null)
                return;
            timeSinceLastUpdate += (float)GetPhysicsProcessDeltaTime();
            if (!hasTargetPosition || timeSinceLastUpdate >= pathUpdateInterval)
            {
                Vector2 target = player.GlobalPosition;
                if (!hasTargetPosition || TargetPosition != target)
                    TargetPosition = target;
                timeSinceLastUpdate = 0.0f;
                hasTargetPosition = true;
            }
            MoveAlongCalculatedPath(player.GlobalPosition);
        }

        public void PathfindToPoint(Vector2 target)
        {
            if (characterBody == null || velocityComponent == null) return;
            if (!hasTargetPosition || TargetPosition != target)
            {
                TargetPosition = target;
                hasTargetPosition = true;
            }
            MoveAlongCalculatedPath(target);
        }

        private void MoveAlongCalculatedPath(Vector2 finalTarget)
        {
            if (IsTargetReached())
            {
                targetReached = true;
                velocityComponent.Stop();
                return;
            }

            targetReached = false;
            Vector2 nextPathPosition = GetNextPathPosition();

            Vector2 direction = characterBody.GlobalPosition.DirectionTo(nextPathPosition).Normalized();
            distanceToTarget = characterBody.GlobalPosition.DistanceTo(finalTarget);

            velocityComponent.AccelerateToDirection(direction);
            velocityComponent.Move(direction);
        }

        public float DistanceFromTarget() => distanceToTarget;
        public bool ReachedTarget() => IsTargetReached();
    }
}
