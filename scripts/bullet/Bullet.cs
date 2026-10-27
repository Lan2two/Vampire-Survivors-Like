namespace SurvivorsGame
{
    using Godot;

    public partial class Bullet : Node2D, IWeapon
    {
        [Export] public BulletData config;
        private AttackComponent attackComponent;
        private float traveledDistance = 0;
        private int hitCount = 0;
        private bool Crit;
        private float FinalDamage;

        public override void _Ready()
        {
            attackComponent = GetNodeOrNull<AttackComponent>("AttackComponent");
            if (attackComponent == null)
            {
                GD.PrintErr("AttackComponent not found in Bullet");
            }
        }

        public void Use(UpgradeManager upgradeManager)
        {
            CallDeferred(MethodName.UpdateDamage);
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

        private void UpdateDamage()
        {
            int Randint = GD.RandRange(0, 100);
            Crit = false;
            FinalDamage = config.Damage;
            if (Randint < config.CritRate)
            {
                Crit = true;
                FinalDamage *= config.CritDamageMultiplier;
            }
            attackComponent.damage = FinalDamage;
            attackComponent.knockback = config.Knockback;
            attackComponent.crit = Crit;
        }
    }
}
