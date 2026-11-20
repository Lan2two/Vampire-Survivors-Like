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
    [Export] public UpgradeType upgradeType { get; set; }

}