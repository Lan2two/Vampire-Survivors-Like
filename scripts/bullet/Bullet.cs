namespace SurvivorsGame
{
    using System;
    using Godot;

    public partial class Bullet : Weapon, IPoolable
    {
        public ProjectileData config;
        public Line2D trail;
        private Sprite2D sprite2D;
        private RedirectBullet redirectBullet;
        private float traveledDistance = 0f;
        private int hitCount = 0;
        private bool Crit;
        private float FinalDamage;

        public Action<Bullet> ReturnToPool { get; set; }

        public override void _Ready()
        {
            base._Ready();
            trail = GetNodeOrNull<Line2D>("Sprite2D/Trail");
            sprite2D = GetNodeOrNull<Sprite2D>("Sprite2D");
            redirectBullet = GetNodeOrNull<RedirectBullet>("RedirectBullet");
            if (sprite2D == null)
            {
                GD.PrintErr("Sprite2D not found in Bullet");
            }
            if (redirectBullet == null)
            {
                GD.PrintErr("RedirectBullet not found in Bullet");
            }
        }

        public void OnSpawn()
        {
            if (config == null || attackComponent == null || sprite2D == null)
            {
                GD.PrintErr($"Cannot Use Bullet: config or attackComponent is null on {Name}!");
                return;
            }
            trail?.ClearPoints();
            if (config.projectileTexture != null)
            {
                sprite2D.Texture = config.projectileTexture;
            }

            traveledDistance = 0f;
            hitCount = 0;
            attackComponent.ClearHash();

            Show();
            ProcessMode = ProcessModeEnum.Inherit;
            SetAreaActive(attackComponent, true);
            SetAreaActive(redirectBullet, true);

            CallDeferred(Weapon.MethodName.UpdateDamage, config, attackComponent);
            if (config.GetStat("Bounce").CalculateFinal() > 0)
            {
                redirectBullet.Redirect = true;
            }
        }


        public override void _PhysicsProcess(double delta)
        {
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
