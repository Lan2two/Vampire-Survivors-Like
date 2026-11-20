using Godot;
using System;


public partial class MeleeWeapon : Node2D, IWeapon, IWeaponConfigurable
{
    public MeleeWeaponBase Baseconfig;
    [Export] DamageComponent damageComponent;
    [Export] bool SwingVariant;
    AnimationPlayer animationPlayer;
    public MeleeWeaponBase config;
    private double timer = 0;
    private bool swingForward = true;



    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.AnimationFinished += OnAnimationFinished;
        animationPlayer.AnimationStarted += OnAnimationStarted;
        SetDamageNodeEnabled(false);
    }

    public override void _ExitTree()
    {
        animationPlayer.AnimationFinished -= OnAnimationFinished;
        animationPlayer.AnimationStarted -= OnAnimationStarted;
    }
    public void Use(UpgradeManager upgradeManager)
    {
        if (timer > 0)
        {
            return;
        }

        config = (MeleeWeaponBase)Baseconfig.Duplicate();
        upgradeManager.ApplyUpgrades(config, UpgradeType.Melee);
        UpdateDamage();

        Swing();
    }

    public void Configure(WeaponBase weaponData)
    {
        if (weaponData is not MeleeWeaponBase meleeWeaponData)
        {
            throw new ArgumentException("MeleeWeapon requires MeleeWeaponBase data.", nameof(weaponData));
        }

        Baseconfig = meleeWeaponData;
    }
    public override void _PhysicsProcess(double delta)
    {
        timer -= delta;
    }

    public void UpdateDamage()
    {
        damageComponent.damage = config.BaseDamage;
        damageComponent.knockback = config.BaseKnockback;
    }
    private void Swing()
    {
        if (timer > 0)
        {
            return;
        }
        float speedMultiplier = config.SwingSpeedMultiplier;
        timer = config.swingCooldown / speedMultiplier;
        animationPlayer.SpeedScale = speedMultiplier;
        GD.Print($"Swinging weapon with speed multiplier: {speedMultiplier}, cooldown: {config.swingCooldown}, timer set to: {timer}");

        if (SwingVariant)
        {
            if (swingForward)
            {
                animationPlayer.Play("swing");
            }
            else
            {
                animationPlayer.PlayBackwards("swing");
            }
        }
        else
        {
            animationPlayer.Play("swing");
        }

        swingForward = !swingForward;
        SetDamageNodeEnabled(true);
    }
    private void OnAnimationFinished(StringName animName)
    {
        if (animName != "swing")
        {
            return;
        }

        SetDamageNodeEnabled(false);
        damageComponent.ClearHash();
    }
    private void OnAnimationStarted(StringName animName)
    {
        if (animName != "swing")
        {
            return;
        }

        SetDamageNodeEnabled(true);
    }

    private void SetDamageNodeEnabled(bool enabled)
    {
        if (damageComponent == null)
        {
            return;
        }

        damageComponent.SetDeferred("monitoring", enabled);
        damageComponent.SetPhysicsProcess(enabled);
    }
}
