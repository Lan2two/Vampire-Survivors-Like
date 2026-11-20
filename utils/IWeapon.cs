using Godot;

public interface IWeapon
{
    void Use(UpgradeManager upgradeManager);
}

public interface IWeaponConfigurable
{
    void Configure(WeaponBase weaponData);
}
