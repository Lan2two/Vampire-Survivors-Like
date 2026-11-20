namespace SurvivorsGame
{
    using Godot;
    [GlobalClass]
    public partial class WeaponBase : ItemData, IDamageDealing
    {
        private float _baseDamage = 10f;
        private float _baseKnockback = 100f;
        private float _critRate = 0;
        private float _critDamageMultiplier = 1.2f;
        [Export] public PackedScene WeaponVisualScene;
        [Export]
        public float Damage
        {
            get => _baseDamage;
            set => _baseDamage = Mathf.Clamp(value, CombatStatLimits.MinDamage, CombatStatLimits.MaxDamage);
        }
        [Export]
        public float Knockback
        {
            get => _baseKnockback;
            set => _baseKnockback = Mathf.Clamp(value, CombatStatLimits.MinKnockback, CombatStatLimits.MaxKnockback);
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
