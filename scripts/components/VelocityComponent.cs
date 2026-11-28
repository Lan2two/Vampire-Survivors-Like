namespace SurvivorsGame
{
    using Godot;
    using System.Collections.Generic;

    [GlobalClass]
    public partial class VelocityComponent : Node
    {
        [Export] public RigidBody2D rigidBody;
        [Export] public AnimatedSprite2D animatedSprite2D;
        [Export] public float maxSpeed = 100.0f;
        [Export] public float acceleration = 40.0f;
        [Export] private bool SpeedVariation = false;

        public Vector2 Velocity;

        public float CurrentMaxSpeed { get; private set; }
        public float CurrentAcceleration { get; private set; }

        private readonly Dictionary<string, (float speedMult, float accelMult)> _modifiers = new();

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
                maxSpeed += (float)GD.RandRange(-0.05f, 0.05f) * maxSpeed;
            }

            RecalculateStats();
        }

        public void SetModifier(string source, float speedMult = 1f, float accelMult = 1f)
        {
            _modifiers[source] = (speedMult, accelMult);
            RecalculateStats();
        }

        public void RemoveModifier(string source)
        {
            if (_modifiers.Remove(source))
            {
                RecalculateStats();
            }
        }

        private void RecalculateStats()
        {
            float totalSpeedMult = 1f;
            float totalAccelMult = 1f;

            foreach (var mod in _modifiers.Values)
            {
                totalSpeedMult *= mod.speedMult;
                totalAccelMult *= mod.accelMult;
            }

            CurrentMaxSpeed = maxSpeed * totalSpeedMult;
            CurrentAcceleration = acceleration * totalAccelMult;
        }

        public void AccelerateToVelocity(Vector2 velocity)
        {
            float step = Mathf.Clamp(CurrentAcceleration * (float)GetPhysicsProcessDeltaTime(), 0f, 1f);
            Velocity = Velocity.MoveToward(velocity, step * CurrentMaxSpeed);
        }

        public void AccelerateToDirection(Vector2 direction)
        {
            float step = Mathf.Clamp(CurrentAcceleration * (float)GetPhysicsProcessDeltaTime(), 0f, 1f);
            Velocity = Velocity.MoveToward(direction * CurrentMaxSpeed, step * CurrentMaxSpeed);
        }

        public Vector2 GetMaxVelocity()
        {
            return Velocity;
        }

        public void Decelerate()
        {
            float step = Mathf.Clamp(CurrentAcceleration * (float)GetPhysicsProcessDeltaTime(), 0f, 1f);
            Velocity = Velocity.MoveToward(Vector2.Zero, step * CurrentAcceleration);
        }

        public void Stop()
        {
            Velocity = Vector2.Zero;
        }

        public void StopImmediately()
        {
            Stop();
            if (rigidBody != null && IsInstanceValid(rigidBody))
            {
                rigidBody.LinearVelocity = Vector2.Zero;
            }
        }

        public void ApplyVelocityChange(Vector2 velocityChange)
        {
            if (rigidBody == null || !IsInstanceValid(rigidBody)) return;
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
            if (rigidBody == null || !IsInstanceValid(rigidBody)) return;

            if (delta > 0.0)
            {
                float maxAcceleration = Mathf.Max(0.0f, CurrentMaxSpeed * CurrentAcceleration);
                Vector2 velocityError = Velocity - rigidBody.LinearVelocity;
                Vector2 desiredAcceleration = (velocityError / (float)delta).LimitLength(maxAcceleration);
                rigidBody.ApplyCentralForce(desiredAcceleration * rigidBody.Mass);
            }

            Decelerate();
        }
    }
}
