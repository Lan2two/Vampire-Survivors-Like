using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class InventoryManager : Node
{
    [Export] public Array<Strategy> Upgrades;
    [Export] public WeaponController weaponController;
    [Export] public PackedScene WorldItemScene;

    public Node weaponInstance;
    public WeaponBase currentWeapon;

    [Export] public Array<ItemData> Slots = new();
    [Export] public int MaxSlots = 8;

    public override void _Ready()
    {
    }

    public bool AddItem(ItemData itemData)
    {
        if (itemData == null)
        {
            return false;
        }

        if (itemData is WeaponBase weapon)
        {
            SwapWeapon(weapon);
            return true;
        }

        if (itemData is Strategy strategy)
        {
            Upgrades.Add(strategy);
            return true;
        }

        if (Slots.Count >= MaxSlots)
        {
            GD.Print("Inventory full, cannot pick up: " + itemData.ItemName);
            return false;
        }

        Slots.Add(itemData);
        return true;
    }

    public void SwapWeapon(WeaponBase newWeapon)
    {
        if (newWeapon == null)
        {
            return;
        }

        if (currentWeapon != null)
        {
            DropCurrentWeapon();
        }

        EquipWeapon(newWeapon);
    }

    public void EquipWeapon(WeaponBase weapon)
    {
        if (weapon == null)
        {
            return;
        }

        weaponInstance = weapon.WeaponVisualScene.Instantiate();
        weaponController.handposition.AddChild(weaponInstance);
        weaponController.weapon = weaponInstance as IWeapon;
        currentWeapon = weapon;
        GD.Print("Equipped: " + weapon.ItemName);
    }

    public void DropCurrentWeapon()
    {
        if (currentWeapon == null || weaponInstance == null)
        {
            return;
        }

        WeaponBase droppedWeapon = currentWeapon;

        weaponInstance.QueueFree();
        weaponInstance = null;
        weaponController.weapon = null;
        currentWeapon = null;

        SpawnWorldItem(droppedWeapon);
    }

    public void DropWeapon()
    {
        DropCurrentWeapon();
    }

    // Drops a non-weapon item out of a slot back into the world.
    public void DropItem(ItemData itemData)
    {
        if (itemData == null || !Slots.Contains(itemData))
        {
            return;
        }

        Slots.Remove(itemData);
        SpawnWorldItem(itemData);
    }

    private void SpawnWorldItem(ItemData itemData)
    {
        if (WorldItemScene == null)
        {
            GD.PrintErr("InventoryManager.WorldItemScene not assigned; cannot drop item.");
            return;
        }

        Node worldItemNode = WorldItemScene.Instantiate();
        if (worldItemNode is WorldItem worldItem)
        {
            worldItem.ItemData = itemData;
        }

        Player player = this.GetPlayer();
        GetTree().CurrentScene.AddChild(worldItemNode);

        if (worldItemNode is Node2D node2D && player != null)
        {
            Vector2 dropOffset = new Vector2(12, 0).Rotated((float)GD.RandRange(0.0, Mathf.Tau));
            node2D.GlobalPosition = player.GlobalPosition + dropOffset;
        }
    }
}
