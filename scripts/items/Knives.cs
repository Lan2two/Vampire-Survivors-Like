using Godot;

namespace SurvivorsGame
{
    public partial class Knives : ProjectileLauncher
    {
        public override void _Ready()
        {
            base._Ready();
            CallDeferred(MethodName.OnUpgrade);
            if (upgradeManager == null)
            {
                upgradeManager = this.GetPlayer().upgradeManager;
            }
            EventBus.KnivesUpgrade += OnUpgrade;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            EventBus.KnivesUpgrade -= OnUpgrade;
        }

        private void OnUpgrade()
        {
            config = (ProjectileData)startingConfig.Duplicate();
            upgradeManager.ApplyUpgrades(this, UpgradeType.Knives);
            timer.WaitTime = config.GetStat("ProjectileCooldown").CalculateFinal();
        }
    }
}