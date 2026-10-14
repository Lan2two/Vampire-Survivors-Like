namespace SurvivorsGame
{
	using Godot;
	using System;

	public partial class Player : RigidBody2D
	{
		[Export] public float playerXP = 0.0f;
		public AnimatedSprite2D anim;
		public InventoryManager inventoryManager;
		public HealthComponent healthComponent;
		public float totalXP;

		public event Action PlayerDead;
		public override void _Ready()
		{
			anim = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
			inventoryManager = GetNodeOrNull<InventoryManager>("InventoryManager");
			healthComponent = GetNodeOrNull<HealthComponent>("HealthComponent");
			if (anim == null)
			{
				GD.PrintErr("AnimatedSprite2D node not found in Player");
			}
			if (inventoryManager == null)
			{
				GD.PrintErr("InventoryManager node not found in Player");
			}
			healthComponent.Damage += CheckDeath;
		}

		private void CheckDeath(Attack attack)
		{
			if (healthComponent.currentHealth <= 0)
			{
				PlayerDead?.Invoke();
			}
		}
	}
}
