using Godot;
using System;
using System.Linq;

public partial class WeaponController : Node2D
{
    [Export] StateMachine stateMachine;
    [Export] UpgradeManager upgradeManager;
    public Marker2D handposition;
    public IWeapon weapon;

    public override void _Ready()
    {
        handposition = GetNode<Marker2D>("HandPosition");
        weapon = handposition.GetChildren().OfType<IWeapon>().FirstOrDefault();

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
        if (weapon is MeleeWeapon melee)
        {
            melee.config = (MeleeWeaponData)melee.Baseconfig.Duplicate();
            upgradeManager.UpgradeMelee(melee);
            melee.UpdateDamage();
        }
        weapon.Use();
    }

    private void AimRotationCursor()
    {
        Vector2 mousePosition = GetGlobalMousePosition();
        handposition.LookAt(mousePosition);
    }
}
