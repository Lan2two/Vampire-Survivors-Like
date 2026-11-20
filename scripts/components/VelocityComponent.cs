namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class VelocityComponent : Node2D
    {
        [Export] public RigidBody2D rigidBody;
        [Export] public AnimatedSprite2D animatedSprite2D;
        [Export] public float maxSpeed = 100.0f;
        [Export] public float acceleration = 40.0f;
        [Export] private bool SpeedVariation = false;

        public Vector2 Velocity;

        public override void _Ready()
        {
            if (rigidBody == null)
            {
                GD.PrintErr($"{Name}: RigidBody2D is not assigned.");
                SetPhysicsProcess(false);
                return;
            }

            rigidBody.GravityScale = 0.0f;
            rigidBody.LockRotation = true;
            if (SpeedVariation)
            {
                maxSpeed += GD.RandRange(-5, 5) / 10 * maxSpeed;
            }
        }

        public void AccelerateToVelocity(Vector2 velocity)
        {
            float step = Mathf.Clamp(acceleration * (float)GetPhysicsProcessDeltaTime(), 0f, 1f);
            Velocity = Velocity.MoveToward(velocity, step * maxSpeed);
        }

        public void AccelerateToDirection(Vector2 direction)
        {
            float step = Mathf.Clamp(acceleration * (float)GetPhysicsProcessDeltaTime(), 0f, 1f);
            Velocity = Velocity.MoveToward(direction * maxSpeed, step * maxSpeed);
        }

        public Vector2 GetMaxVelocity()
        {
            return Velocity;
        }

        public void Decelerate()
        {
            float step = Mathf.Clamp(acceleration * (float)GetPhysicsProcessDeltaTime(), 0f, 1f);
            Velocity = Velocity.MoveToward(Vector2.Zero, step * acceleration);
        }

        public void Stop()
        {
            Velocity = Vector2.Zero;
        }

        public void ApplyVelocityChange(Vector2 velocityChange)
        {
            if (rigidBody == null || !IsInstanceValid(rigidBody))
            {
                return;
            }

            rigidBody.ApplyCentralImpulse(velocityChange * rigidBody.Mass);
        }

        public void Move(Vector2 direction)
        {
            if (direction.X != 0 && animatedSprite2D != null)
            {
                animatedSprite2D.FlipH = direction.X < 0;
            }

            if (direction == Vector2.Zero)
            {
                Stop();
                return;
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            if (rigidBody == null || !IsInstanceValid(rigidBody))
            {
                return;
            }

            if (delta > 0.0)
            {
                float maxAcceleration = Mathf.Max(0.0f, maxSpeed * acceleration);
                Vector2 velocityError = Velocity - rigidBody.LinearVelocity;
                Vector2 desiredAcceleration = (velocityError / (float)delta).LimitLength(maxAcceleration);
                rigidBody.ApplyCentralForce(desiredAcceleration * rigidBody.Mass);
            }

            Decelerate();
        }

    }
}
