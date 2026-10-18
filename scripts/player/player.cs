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
		public Roll rollState;
		public float totalXP;
		public event Action PlayerDead;
		public override void _Ready()
		{
			anim = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
			inventoryManager = GetNodeOrNull<InventoryManager>("InventoryManager");
			healthComponent = GetNodeOrNull<HealthComponent>("HealthComponent");
			rollState = GetNodeOrNull<Roll>("StateMachine/roll");
			if (anim == null)
			{
				GD.PrintErr("AnimatedSprite2D node not found in Player");
				return;
			}
			if (inventoryManager == null)
			{
				GD.PrintErr("InventoryManager node not found in Player");
				return;
			}
			if (healthComponent == null)
			{
				GD.PrintErr("HealthComponent node not found in Player");
				return;
			}
			if (rollState == null)
			{
				GD.PrintErr("Roll State node not found in Player");
				return;
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
