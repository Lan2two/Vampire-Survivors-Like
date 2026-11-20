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
        public virtual UpgradeType upgradeType { get; set; }

    }
}
