using Godot;
using Godot.Collections;
using System;

public partial class InventoryManager : Node
{
    [Export] public Array<Strategy> Upgrades;
    [Export] public WeaponController weaponController;

    public void SwitchWeapon(PackedScene weapon)
    {
        Node weaponInstance = weapon.Instantiate();
        if (weapon == null)
        {
            return;
        }
        foreach (Node child in weaponController.handposition.GetChildren())
        {
            child.QueueFree();
        }
        weaponController.handposition.AddChild(weaponInstance);
        weaponController.weapon = weaponInstance as IWeapon;
    }
}
