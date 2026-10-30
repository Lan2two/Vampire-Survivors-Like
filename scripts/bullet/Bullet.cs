namespace SurvivorsGame
{
    using Godot;

    public partial class Bullet : Weapon
    {
        [Export] public BulletData config;
        private RedirectBullet redirectBullet;
        private float traveledDistance = 0;
        private int hitCount = 0;
        private bool Crit;
        private float FinalDamage;

        public override void _Ready()
        {
            redirectBullet = GetNodeOrNull<RedirectBullet>("RedirectBullet");
            if (redirectBullet == null)
            {
                GD.PrintErr("RedirectBullet not found in Redirect Bullet");
            }
        }

        public void Fire()
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
            if (config.BouncingBullets)
            {
                redirectBullet.Redirect = true;
            }
            Vector2 direction = Vector2.Right.Rotated(Rotation).Normalized();
            Position += direction * config.Speed * (float)delta;
            traveledDistance += config.Speed * (float)delta;

            if (config.MaxTravelDistance < traveledDistance || attackComponent.GetHurtBoxEntered() > config.PierceCount)
            {
                QueueFree();
            }
        }
    }
}
