namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class BulletData : Resource, IDamageDealing
    {
        private float _damage = 10f;
        private float _knockback = 100f;
        private float _speed = 300f;
        private int _pierceCount = 0;
        private float _gunCooldown = 1f;
        private float _critRate = 0;
        private float _critDamageMultiplier = 1.2f;
        [Export]
        public float Damage
        {
            get => _damage;
            set => _damage = Mathf.Clamp(value, CombatStatLimits.MinDamage, CombatStatLimits.MaxDamage);
        }
        [Export]
        public float Knockback
        {
            get => _knockback;
            set => _knockback = Mathf.Clamp(value, CombatStatLimits.MinKnockback, CombatStatLimits.MaxKnockback);
        }
        [Export]
        public float Speed
        {
            get => _speed;
            set => _speed = Mathf.Clamp(value, CombatStatLimits.MinBulletSpeed, CombatStatLimits.MaxBulletSpeed);
        }
        [Export] public float MaxTravelDistance { get; set; } = 500f;
        [Export]
        public int PierceCount
        {
            get => _pierceCount;
            set => _pierceCount = Mathf.Max(value, 0);
        }
        [Export]
        public float GunCooldown
        {
            get => _gunCooldown;
            set => _gunCooldown = (float)Mathf.Max(value, 0.001);
        }
        [Export]
        public float CritRate
        {
            get => _critRate;
            set => _critRate = value;
        }
        [Export]
        public float CritDamageMultiplier
        {
            get => _critDamageMultiplier;
            set => _critDamageMultiplier = value;
        }
    }
}
