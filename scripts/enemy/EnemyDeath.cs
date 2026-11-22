namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class EnemyDeath : Interrupt
    {
        [Export] RigidBody2D characterBody;
        [Export] HurtboxComponent _hurtbox;
        [Export] HealthComponent healthComponent;
        [Export] VelocityComponent velocityComponent;
        [Export] AttackComponent attackComponent;
        [Export] AnimatedSprite2D Sprite;

        public event Action OnDeath;
        private bool _isDead;

        private uint _defaultBodyCollisionLayer;
        private uint _defaultAttackCollisionMask;
        private bool _defaultsCached = false;

        private void CacheDefaults()
        {
            if (_defaultsCached) return;
            if (characterBody != null)
            {
                _defaultBodyCollisionLayer = characterBody.CollisionLayer;
            }
            if (attackComponent != null)
            {
                _defaultAttackCollisionMask = attackComponent.CollisionMask;
            }
            _defaultsCached = true;
        }

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
            if (_hurtbox == null)
            {
                GD.PrintErr("Hurtbox not assigned in DeathComponent.");
                return;
            }

            CacheDefaults();

            Sprite.AnimationFinished += OnAnimationFinished;
            healthComponent.FinalDamageEvent += CheckforDeath;
        }

        public void Reset()
        {
            CacheDefaults();
            _isDead = false;
            _hurtbox?.SetInvincibility(false);

            if (Sprite != null && Sprite.Animation == "die")
            {
                Sprite.Stop();
            }

            if (attackComponent != null && _defaultAttackCollisionMask != 0)
            {
                attackComponent.CollisionMask = _defaultAttackCollisionMask;
            }
            if (characterBody != null && _defaultBodyCollisionLayer != 0)
            {
                characterBody.CollisionLayer = _defaultBodyCollisionLayer;
            }
        }

        private void CheckforDeath(Attack attackData)
        {
            if (_isDead || healthComponent.currentHealth > 0 || (characterBody is Enemy enemy && !enemy.IsActive))
            {
                return;
            }
            _isDead = true;
            FireInterrupt();
        }

        public override void Enter()
        {
            Stats.data.EnemiesKilled++;
            _hurtbox.SetInvincibility(true);
            attackComponent?.SetDeferred(CollisionObject2D.PropertyName.CollisionMask, 0);
            characterBody?.SetDeferred(CollisionObject2D.PropertyName.CollisionLayer, 0);
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
                healthComponent.FinalDamageEvent -= CheckforDeath;
            }
        }

        private void OnAnimationFinished()
        {
            if (Sprite.Animation != "die")
            {
                return;
            }
            OnDeath?.Invoke();

            if (characterBody is Enemy enemy)
            {
                enemy.Despawn();
            }
            else
            {
                characterBody?.QueueFree();
            }
        }
    }
}
