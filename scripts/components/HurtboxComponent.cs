namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class HurtboxComponent : Area2D
    {
        [Export] public HealthComponent healthComponent;
        [Export] public double iFrames = 0.1;
        private double _iFramesTimer;
        private CollisionShape2D _hurtboxShape;
        public override void _Ready()
        {
            _hurtboxShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
            if (_hurtboxShape == null)
            {
                GD.PrintErr("HurtboxShape not found in HurtboxComponent");
                return;
            }
            _iFramesTimer = iFrames;
        }

        public override void _PhysicsProcess(double delta)
        {
            _iFramesTimer -= delta;
            if (_iFramesTimer > 0)
            {
                return;
            }
            SetiFrames(false);
        }

        public void TakeDamage(Attack attackData)
        {
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(attackData);
                SetiFrames(true);
                _iFramesTimer = iFrames;
            }
        }

        private void SetiFrames(bool state)
        {
            _hurtboxShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, state);
        }
    }
}

