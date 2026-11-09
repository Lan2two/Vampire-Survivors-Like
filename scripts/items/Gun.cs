namespace SurvivorsGame
{
    public partial class Gun : ProjectileLauncher
    {
        public override void _Ready()
        {
            base._Ready();
            CallDeferred(MethodName.OnUpgrade);
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
            upgradeManager.ApplyUpgrades(this, UpgradeType.Projectile);
            timer.WaitTime = config.GetStat("ProjectileCooldown").CalculateFinal();
        }
    }
}