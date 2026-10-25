namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class Enemy : RigidBody2D
    {
        [Export] public float XpAmount = 1f;
        public AnimatedSprite2D animatedSprite;
        public CollisionShape2D collisionbox;
        private HealthComponent healthComponent;
        public static int Alive = 0;
        public override void _Ready()
        {
            animatedSprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
            collisionbox = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
            healthComponent = GetNodeOrNull<HealthComponent>("HealthComponent");
            if (animatedSprite == null)
            {
                GD.PrintErr("AnimatedSprite2D not found in Enemy");
                return;
            }
            if (collisionbox == null)
            {
                GD.PrintErr("CollisionShape2D not found in Enemy");
                return;
            }
            if (healthComponent == null)
            {
                GD.PrintErr("HealthComponent not found in Enemy");
                return;
            }
            healthComponent.Damage += TrackDamage;
            Alive++;
        }
        public override void _ExitTree()
        {
            if (healthComponent != null)
            {
                healthComponent.Damage -= TrackDamage;
            }
            Alive--;
        }

        private void TrackDamage(Attack attack)
        {
            Stats.data.DamageDealt += attack.Damage;
        }
    }
}
