namespace SurvivorsGame
{
    using Godot;

    public partial class Bullet : Weapon, IWeapon
    {
        [Export] public BulletData config;
        private float traveledDistance = 0;
        private int hitCount = 0;
        private bool Crit;
        private float FinalDamage;

        public void Use()
        {
            if (config == null || attackComponent == null)
            {
                GD.PrintErr($"Cannot Use Bullet: config or attackComponent is null on {Name}!");
                return;
            }
            CallDeferred(Weapon.MethodName.UpdateDamage, config, attackComponent);
        }

        public override void _PhysicsProcess(double delta)
        {
            Vector2 direction = Vector2.Left.Rotated(Rotation).Normalized();
            Position += direction * config.Speed * (float)delta;
            traveledDistance += config.Speed * (float)delta;

            if (config.MaxTravelDistance < traveledDistance || attackComponent.GetDamagedHurtBoxes() > config.PierceCount)
            {
                QueueFree();
            }
        }
    }
}
