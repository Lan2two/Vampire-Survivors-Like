namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class EnemyDeath : Interrupt
    {
        [Export] RigidBody2D characterBody;
        [Export] CollisionShape2D _hurtboxShape;
        [Export] HealthComponent healthComponent;
        [Export] VelocityComponent velocityComponent;
        [Export] AttackComponent attackComponent;
        [Export] AnimatedSprite2D Sprite;
        public event Action OnDeath;
        private bool _isDead;
        public override void _Ready()
        {
            if (Sprite == null)
            {
                GD.PrintErr("Sprite not assigned in DeathComponent.");
                return;
            }
            if (healthComponent == null)
            {
                GD.PrintErr("HealthComponent not assigned in DeathComponent.");
                return;
            }
            if (_hurtboxShape == null)
            {
                GD.PrintErr("Hurtbox not assigned in DeathComponent.");
                return;
            }
            Sprite.AnimationFinished += OnAnimationFinished;
            healthComponent.Damage += CheckforDeath;
        }
        private void CheckforDeath(Attack attackData)
        {
            if (_isDead || healthComponent.currentHealth > 0)
            {
                return;
            }
            _isDead = true;
            FireInterrupt();
        }

        public override void Enter()
        {
            Stats.data.EnemiesKilled++;
            _hurtboxShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
            attackComponent?.SetDeferred("collision_mask", 0);
            characterBody?.SetDeferred("collision_layer", 0);
            Sprite.Play("die");
        }

        public override void PhysicsUpdate(double delta)
        {
            velocityComponent.Stop();
        }

        public override void _ExitTree()
        {
            if (Sprite != null)
            {
                Sprite.AnimationFinished -= OnAnimationFinished;
            }
            if (healthComponent != null)
            {
                healthComponent.Damage -= CheckforDeath;
            }
        }
        private void OnAnimationFinished()
        {
            if (Sprite.Animation != "die")
            {
                return;
            }
            OnDeath?.Invoke();
            characterBody.QueueFree();
        }
    }
}
