namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class ProjectilePierceStrategy : Strategy
    {
        [Export] int UpgradePierce { get; set; } = 1;

        public ProjectilePierceStrategy()
        {
            upgradeType = UpgradeType.Gun;
            ItemName = "Piercing Rounds";
            ItemDescription = "Bullet pierces";
        }

        public override void ApplyUpgrade(IUpgradable upgradable)
        {
            Resource config = upgradable.currentConfig;
            if (config is ProjectileData bulletData)
            {

                bulletData.PierceCountAdditive += UpgradePierce;
            }
        }
    }
}
