using Godot;

namespace SurvivorsGame
{
    public interface IWeapon
    {
        void Use();
    }

    public interface IWeaponConfigurable
    {
        void Configure(WeaponBase weaponData);
    }
}
