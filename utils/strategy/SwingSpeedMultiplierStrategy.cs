using Godot;

[GlobalClass]
public partial class SwingSpeedMultiplierStrategy : Strategy, IStrategy
{
    [Export] float UpgradeSwingSpeed { get; set; } = 5f;
    public void ApplyUpgrade(Resource config)
    {
        if (config is MeleeWeaponBase swingSpeedMultiplier)
        {
            swingSpeedMultiplier.SwingSpeedMultiplier += UpgradeSwingSpeed;
        }
    }
}