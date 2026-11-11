namespace SurvivorsGame
{
    using Godot;

    public partial class Roll : States
    {
        //highkey a redundant state as it used to overwite player movement, but now just added speed and i frames
        [Export] private VelocityComponent velocityComponent;
        [Export] public double rollCooldown = 3f;
        [Export] public float rollSpeed = 1.5f;
        public double timer = 0; //internal timer for roll cooldown

        public override void Enter()
        {
            this.GetPlayer().animationPlayer.AnimationFinished += OnAnimationFinished;
            this.GetPlayer().animationPlayer.Play("roll");
        }

        public override void Exit()
        {
            this.GetPlayer().animationPlayer.AnimationFinished -= OnAnimationFinished;
        }

        public override void PhysicsUpdate(double delta)
        {
            Vector2 direction = Input.GetVector("left", "right", "up", "down") * rollSpeed;
            velocityComponent.AccelerateToDirection(direction);
            velocityComponent.Move(direction);
        }


        private void OnAnimationFinished(StringName animName)
        {
            timer = rollCooldown;
            ChangeState("Idle");
        }
    }
}
