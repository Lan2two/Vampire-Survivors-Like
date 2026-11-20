namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class MeleeWeaponBase : WeaponBase
    {
        private float _swingSpeedMultiplier = 1.0f;

        [Export]
        public float SwingSpeedMultiplier
        {
            get => _swingSpeedMultiplier;
            set => _swingSpeedMultiplier = Mathf.Clamp(
                value,
                CombatStatLimits.MinMeleeSwingSpeed,
                CombatStatLimits.MaxMeleeSwingSpeed);
        }

        [Export] public float swingCooldown { get; set; } = 1.0f;
    }
}
