using System;
using Godot;


public enum UpgradeType
{
    Bullet,
    Melee
}
[GlobalClass]
public partial class Strategy : Resource
{
    [Export] public UpgradeType upgradeType { get; set; }
    [Export] Texture2D Texture { get; set; }
    [Export] string UpgradeText { get; set; } = "Damage";

}