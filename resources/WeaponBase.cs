namespace SurvivorsGame
{
    using Godot;
    [GlobalClass]
    public partial class WeaponBase : ItemData
    {
        private float _baseDamage = 10f;
        private float _baseKnockback = 100f;
        [Export]
        public float BaseDamage
        {
            get => _baseDamage;
            set => _baseDamage = Mathf.Clamp(value, CombatStatLimits.MinDamage, CombatStatLimits.MaxDamage);
        }
        [Export]
        public float BaseKnockback
        {
            get => _baseKnockback;
            set => _baseKnockback = Mathf.Clamp(value, CombatStatLimits.MinKnockback, CombatStatLimits.MaxKnockback);
        }
        [Export] public PackedScene WeaponVisualScene;
    }
}
