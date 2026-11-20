namespace SurvivorsGame
{
    public partial class Knives : ProjectileLauncher
    {
        public override void _Ready()
        {
            base._Ready();
            CallDeferred(MethodName.OnUpgrade);
            EventBus.KnivesUpgrade += OnUpgrade;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            EventBus.KnivesUpgrade -= OnUpgrade;
        }

        private void OnUpgrade()
        {
            ApplyStrategy(UpgradeType.Knives);
        }
    }
}