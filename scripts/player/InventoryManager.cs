namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    public partial class InventoryManager : Node
    {
        [Export] public Array<Strategy> UpgradesQueue = new();
        public Array<Strategy> Upgrades = new();
        [Export] public WeaponController weaponController;
        [Export] public PackedScene WorldItemScene;
        [Export] public Array<ItemData> Slots = new();
        [Export] public int MaxSlots = 8;
        public Node weaponInstance;
        public WeaponBase currentWeapon;

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
                UpgradesQueue.Add(strategy);
                Upgrades.Add(strategy);
                UpgradeManager.InvokeUpgrades(strategy);
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

            if (weapon.WeaponVisualScene == null)
            {
                GD.PrintErr($"Weapon '{weapon.ItemName}' is missing a WeaponVisualScene.");
                return;
            }

            if (weaponController == null || weaponController.handposition == null)
            {
                GD.PrintErr("InventoryManager is missing weaponController or handposition.");
                return;
            }

            weaponInstance = weapon.WeaponVisualScene.Instantiate();
            weaponController.handposition.AddChild(weaponInstance);
            weaponController.weapon = weaponInstance as IWeapon;

            if (weaponInstance is IWeaponConfigurable configurableWeapon)
            {
                configurableWeapon.Configure(weapon);
            }

            currentWeapon = weapon;
            GD.Print("Equipped: " + weapon.ItemName);
        }

        public void DropCurrentWeapon()
        {
            if (currentWeapon == null)
            {
                return;
            }

            WeaponBase droppedWeapon = currentWeapon;

            if (IsInstanceValid(weaponInstance) &&
                weaponInstance is IUpgradable { currentConfig: WeaponBase upgradedWeapon })
            {
                droppedWeapon = upgradedWeapon;
            }

            SpawnWorldItem(droppedWeapon);

            if (IsInstanceValid(weaponInstance))
            {
                weaponInstance.QueueFree();
            }

            weaponInstance = null;
            if (weaponController != null)
            {
                weaponController.weapon = null;
            }
            currentWeapon = null;
        }

        public void DropWeapon()
        {
            DropCurrentWeapon();
        }

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

            Node spawnParent = GetTree().CurrentScene ?? GetTree().Root;
            spawnParent.AddChild(worldItemNode);

            Player player = this.GetPlayer();
            if (worldItemNode is Node2D node2D && IsInstanceValid(player))
            {
                Vector2 dropOffset = new Vector2(12, 0).Rotated((float)GD.RandRange(0.0, Mathf.Tau));
                node2D.GlobalPosition = player.GlobalPosition + dropOffset;
            }
        }
    }
}
