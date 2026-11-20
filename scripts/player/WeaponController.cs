using Godot;
using System;
using System.Linq;

public partial class WeaponController : Node2D
{
    [Export] StateMachine stateMachine;
    [Export] UpgradeManager upgradeManager;
    [Export] InventoryManager inventoryManager;
    public Marker2D handposition;
    public IWeapon weapon;

    public override void _Ready()
    {
        handposition = GetNode<Marker2D>("HandPosition");
    }
    public override void _Process(double delta)
    {
        AimRotationCursor();
        if (Input.IsActionJustPressed("m1"))
        {
            TryForAttack();
        }
        if (Input.IsActionJustPressed("drop_weapon"))
        {
            inventoryManager.DropWeapon();
        }
    }

    private void TryForAttack()
    {
        if (weapon == null)
        {
            return;
        }
        weapon.Use(upgradeManager);
    }

    private void AimRotationCursor()
    {
        Vector2 mousePosition = GetGlobalMousePosition();
        handposition.LookAt(mousePosition);
    }
}
