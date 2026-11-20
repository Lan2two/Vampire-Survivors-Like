namespace SurvivorsGame
{
    public partial class Gun : ProjectileLauncher
    {
        public override void _Ready()
        {
            base._Ready();
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
            ApplyStrategy(UpgradeType.Gun);
        }
    }
}