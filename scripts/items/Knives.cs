namespace SurvivorsGame
{
    public partial class Knives : ProjectileLauncher
    {
        public override void _Ready()
        {
            base._Ready();
            upgradeManager = this.GetPlayer().upgradeManager;
            CallDeferred(MethodName.OnUpgrade);
            EventBus.GunUpgrade += OnUpgrade;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            EventBus.GunUpgrade -= OnUpgrade;
        }

        private void OnUpgrade()
        {
            config = (ProjectileData)startingConfig.Duplicate();
            upgradeManager.ApplyUpgrades(this, UpgradeType.Knives);
            timer.WaitTime = config.GetStat("ProjectileCooldown").CalculateFinal();
        }
    }
}