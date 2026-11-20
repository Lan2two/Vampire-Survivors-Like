using Godot;
using System;

public partial class WeaponController : Node2D
{
    [Export] StateMachine stateMachine;
    [Export] UpgradeManager upgradeManager;
    Marker2D handposition;
    IWeapon weapon;

    public override void _Ready()
    {
        handposition = GetNode<Marker2D>("HandPosition");
        weapon = handposition.GetNode<IWeapon>("MeleeWeapon");

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
        if (weapon is MeleeWeapon melee)
        {
            if (melee.config != null)
            {
                melee.config = (MeleeWeaponData)melee.config.Duplicate();
            }
            else
            {
                melee.config = new MeleeWeaponData();
            }
            upgradeManager.UpgradeMelee(melee);
        }
        weapon.Use();
    }

    private void AimRotationCursor()
    {
        Vector2 mousePosition = GetGlobalMousePosition();
        handposition.LookAt(mousePosition);
    }

}
