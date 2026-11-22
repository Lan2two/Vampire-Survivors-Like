namespace SurvivorsGame
{
    using System;
    using Godot;

    public partial class Bullet : Weapon, IPoolable
    {
        [Export] public ProjectileData config;
        private RedirectBullet redirectBullet;
        private float traveledDistance = 0f;
        private int hitCount = 0;
        private bool Crit;
        private float FinalDamage;

        public Action<Bullet> ReturnToPool { get; set; }

        public override void _Ready()
        {
            base._Ready();
            redirectBullet = GetNodeOrNull<RedirectBullet>("RedirectBullet");
            if (redirectBullet == null)
            {
                GD.PrintErr("RedirectBullet not found in Redirect Bullet");
            }
        }

        public void OnSpawn()
        {
            if (config == null || attackComponent == null)
            {
                GD.PrintErr($"Cannot Use Bullet: config or attackComponent is null on {Name}!");
                return;
            }

            traveledDistance = 0f;
            hitCount = 0;
            attackComponent.ClearHash();

            Show();
            ProcessMode = ProcessModeEnum.Inherit;
            SetAreaActive(attackComponent, true);
            SetAreaActive(redirectBullet, true);

            CallDeferred(Weapon.MethodName.UpdateDamage, config, attackComponent);
        }


        public override void _PhysicsProcess(double delta)
        {
            if (config.BouncingBullets)
            {
                redirectBullet.Redirect = true;
            }
            Vector2 direction = Vector2.Right.Rotated(Rotation).Normalized();
            Position += direction * config.GetStat("Speed").CalculateFinal() * (float)delta;
            traveledDistance += config.GetStat("Speed").CalculateFinal() * (float)delta;

            if (config.GetStat("MaxTravelDistance").CalculateFinal() < traveledDistance || attackComponent.GetHurtBoxEntered() > config.GetStat("PierceCount").CalculateFinal())
            {
                ReturnToPool?.Invoke(this);
            }
        }

        public void OnDespawn()
        {
            Hide();
            ProcessMode = ProcessModeEnum.Disabled;
            SetAreaActive(attackComponent, false);
            SetAreaActive(redirectBullet, false);
        }

        private static void SetAreaActive(Area2D area, bool active)
        {
            if (area == null) return;
            area.SetDeferred(Area2D.PropertyName.Monitoring, active);
        }
    }
}
