using Godot;
using System;


public partial class MeleeWeapon : Node2D
{
    [Export] SpriteFrames spriteFrames;
    [Export] public float swingCooldown;
    [Export] public float hitboxDuration;
    AnimatedSprite2D animatedSprite2D;
    DamageComponent damageComponent;
    private double timer = 0;
    private double hitboxTimer = 0;
    public override void _Ready()
    {
        animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        damageComponent = GetNode<DamageComponent>("DamageComponent");
        animatedSprite2D.SpriteFrames = spriteFrames;
    }

    public override void _PhysicsProcess(double delta)
    {
        timer -= delta;
        hitboxTimer -= delta;

        if (hitboxTimer <= 0)
        {
            damageComponent.SetDeferred("monitoring", false);
        }

        if (Input.IsActionJustPressed("m1") && timer < 0)
        {
            timer = swingCooldown;
            hitboxTimer = hitboxDuration;
            animatedSprite2D.Play("swing");
            damageComponent.SetDeferred("monitoring", true);
        }
    }



}
