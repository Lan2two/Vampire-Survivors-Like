namespace SurvivorsGame
{
    using Godot;
    public partial class Gun : ProjectileLauncher
    {
        public override void _Ready()
        {
            base._Ready();
            OnUpgrade();
            EventBus.BulletUpgrade += OnUpgrade;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            EventBus.BulletUpgrade -= OnUpgrade;
        }

        private void OnUpgrade()
        {
            config = (ProjectileData)startingConfig.Duplicate();
            upgradeManager.ApplyUpgrades(this, UpgradeType.Bullet);
            timer.WaitTime = config.GetStat("ProjectileCooldown").CalculateFinal();
        }
    }
}