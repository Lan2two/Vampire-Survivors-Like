using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class InventoryManager : Node
{
    [Export] public Array<Strategy> Upgrades;
    [Export] public WeaponController weaponController;
    public Node weaponInstance;
    public WeaponBase currentWeapon;
    [Export] public ItemData[] Slots = System.Array.Empty<ItemData>();

    public override void _Ready()
    {
    }

    public void EquipWeapon(WeaponBase weapon)
    {
        if (weapon == null)
        {
            return;
        }
        weaponInstance = weapon.WeaponVisualScene.Instantiate();

        foreach (Node child in weaponController.handposition.GetChildren())
        {
            child.QueueFree();
        }
        weaponController.handposition.AddChild(weaponInstance);
        weaponController.weapon = weaponInstance as IWeapon;
        GD.Print(weapon);
    }

    public void AddItem(ItemData itemData)
    {
        Slots.Append(itemData);
        if (itemData is WeaponBase)
        {
            WeaponBase weapon = itemData as WeaponBase;
            EquipWeapon(weapon);
        }
    }
}
