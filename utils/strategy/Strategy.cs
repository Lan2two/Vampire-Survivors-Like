namespace SurvivorsGame
{
    using System;
    using Godot;


    public enum UpgradeType
    {
        Bullet,
        Melee
    }
    [GlobalClass]
    public partial class Strategy : ItemData
    {
        [Export] public int UpgradePool { get; set; } = 3;
        public UpgradeType upgradeType;
    }
}
