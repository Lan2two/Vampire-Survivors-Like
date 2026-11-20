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
    }

    private void TryForAttack()
    {
        if (weapon == null)
        {
            return;
        }
        if (weapon is not IWeapon)
        {
            return;
        }
        if (weapon is MeleeWeapon meleeWeapon)
        {
            meleeWeapon.config = (MeleeWeaponBase)meleeWeapon.Baseconfig.Duplicate();
            upgradeManager.UpgradeMelee(meleeWeapon);
            meleeWeapon.UpdateDamage();
        }
        weapon.Use();
    }

    private void AimRotationCursor()
    {
        Vector2 mousePosition = GetGlobalMousePosition();
        handposition.LookAt(mousePosition);
    }
}
