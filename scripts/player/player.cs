using Godot;
using System;

[GlobalClass]
public partial class Player : CharacterBody2D
{

	public AnimatedSprite2D anim;
	public override void _Ready()
	{
		anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
}
