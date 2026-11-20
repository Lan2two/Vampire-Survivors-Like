using Godot;
using System;

public partial class Player : CharacterBody2D
{

	public AnimatedSprite2D anim;
	public InventoryManager inventoryManager;
	public override void _Ready()
	{
		anim = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
		inventoryManager = GetNodeOrNull<InventoryManager>("InventoryManager");
		if (anim == null)
		{
			GD.PrintErr("AnimatedSprite2D node not found in Player");
		}
		if (inventoryManager == null)
		{
			GD.PrintErr("InventoryManager node not found in Player");
		}
	}
}
