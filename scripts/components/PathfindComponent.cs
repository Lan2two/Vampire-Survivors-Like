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
        [Export] private bool useNavigation = true;

        private float distanceToTarget;
        private bool targetReached = false;
        private bool hasTargetPosition;
        private Player player;
        private float timeSinceLastUpdate = 0.0f;

        public override void _Ready()
        {
            player = this.GetPlayer();
            if (useNavigation)
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

            if (useNavigation)
            {
                timeSinceLastUpdate += (float)GetPhysicsProcessDeltaTime();
                if (!hasTargetPosition || timeSinceLastUpdate >= pathUpdateInterval)
                {
                    Vector2 target = player.GlobalPosition;
                    if (!hasTargetPosition || TargetPosition != target)
                        TargetPosition = target;
                    timeSinceLastUpdate = 0.0f;
                    hasTargetPosition = true;
                }
            }

            MoveTowardTarget(player.GlobalPosition);
        }

        public void PathfindToPoint(Vector2 target)
        {
            if (characterBody == null || velocityComponent == null)
                return;

            if (useNavigation && (!hasTargetPosition || TargetPosition != target))
            {
                TargetPosition = target;
                hasTargetPosition = true;
            }

            MoveTowardTarget(target);
        }

        private void MoveTowardTarget(Vector2 finalTarget)
        {
            distanceToTarget = characterBody.GlobalPosition.DistanceTo(finalTarget);
            targetReached = useNavigation
                ? IsTargetReached()
                : distanceToTarget <= threshold;

            if (targetReached)
            {
                velocityComponent.Stop();
                return;
            }

            Vector2 nextPathPosition = useNavigation ? GetNextPathPosition() : finalTarget;
            Vector2 direction = characterBody.GlobalPosition.DirectionTo(nextPathPosition).Normalized();
            velocityComponent.AccelerateToDirection(direction);
            velocityComponent.Move(direction);
        }

        public float DistanceFromTarget() => distanceToTarget;
        public bool ReachedTarget() => targetReached;
        public new bool TargetReached() => targetReached;
    }
}
