namespace SurvivorsGame
{
    using Godot;

    public partial class Enemy : RigidBody2D
    {
        public float XpAmount = 1f;
        public float CoinAmount = 5f;
        public AnimatedSprite2D animatedSprite;
        public CollisionShape2D collisionbox;
        public HealthComponent healthComponent;
        public VelocityComponent velocityComponent;
        public static int Alive = 0;
        public override void _Ready()
        {
            animatedSprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
            collisionbox = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
            healthComponent = GetNodeOrNull<HealthComponent>("HealthComponent");
            velocityComponent = GetNodeOrNull<VelocityComponent>("VelocityComponent");
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
            healthComponent.FinalDamageEvent += TrackDamage;
            Alive++;
        }
        public override void _ExitTree()
        {
            if (healthComponent != null)
            {
                healthComponent.FinalDamageEvent -= TrackDamage;
            }
            Alive--;
        }

        public void SetStats(float hp = 15f, float defense = 0, float maxSpeed = 25f, float coinAmount = 2f, float xpAmount = 2f, Color color = default)
        {
            if (healthComponent == null || velocityComponent == null || animatedSprite == null) return;

            if (color == default)
            {
                color = Colors.White;
            }

            healthComponent.maxHealth = hp;
            healthComponent.currentHealth = hp;
            healthComponent.Defense = defense;
            velocityComponent.maxSpeed = maxSpeed;
            Modulate = color;

            CoinAmount = coinAmount;
            XpAmount = xpAmount;
        }

        private void TrackDamage(Attack attack)
        {
            Stats.data.DamageDealt += attack.Damage;
        }
    }
}
