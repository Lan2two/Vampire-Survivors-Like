using Godot;
using System;


public partial class MeleeWeapon : Node2D, IWeapon
{
    [Export] public MeleeWeaponData Baseconfig;
    [Export] DamageComponent damageComponent;
    AnimationPlayer animationPlayer;
    public MeleeWeaponData config;
    private double timer = 0;
    private bool swingForward = true;
    public override void _Ready()
    {
        config = (MeleeWeaponData)Baseconfig.Duplicate();
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
    public void Use()
    {
        Swing();
    }

    public override void _PhysicsProcess(double delta)
    {
        timer -= delta;
    }

    public void UpdateDamage()
    {
        damageComponent.damage = config.Damage;
        damageComponent.knockback = config.Knockback;
    }
    private void Swing()
    {
        if (timer > 0)
        {
            return;
        }
        float speedMultiplier = Math.Abs(config.SwingSpeedMultiplier);
        timer = config.swingCooldown / speedMultiplier;
        animationPlayer.SpeedScale = speedMultiplier;

        if (swingForward)
        {
            animationPlayer.Play("swing");
        }
        else
        {
            animationPlayer.PlayBackwards("swing");
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
