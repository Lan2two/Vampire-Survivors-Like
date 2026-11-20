using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    public AnimatedSprite2D anim;
    public CollisionShape2D collisionbox;
    public override void _Ready()
    {
        anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        collisionbox = GetNode<CollisionShape2D>("CollisionShape2D");
    }
}
