namespace SurvivorsGame
{
	using Godot;
	using System;

	public partial class Player : RigidBody2D
	{
		[Export] public float playerXP = 0.0f;
		public AnimatedSprite2D animatedSprite;
		public AnimationPlayer animationPlayer;
		public InventoryManager inventoryManager;
		public HealthComponent healthComponent;
		public VelocityComponent velocityComponent;
		public Roll rollState;
		public float totalXP;
		public event Action PlayerDead;
		private bool DeathGuard = false;
		public override void _Ready()
		{
			animatedSprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
			animationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
			inventoryManager = GetNodeOrNull<InventoryManager>("InventoryManager");
			healthComponent = GetNodeOrNull<HealthComponent>("HealthComponent");
			velocityComponent = GetNodeOrNull<VelocityComponent>("VelocityComponent");
			rollState = GetNodeOrNull<Roll>("StateMachine/roll");
			if (animatedSprite == null)
			{
				GD.PrintErr("AnimatedSprite2D node not found in Player");
				return;
			}
			if (animationPlayer == null)
			{
				GD.PrintErr("AnimationPlayer node not found in Player");
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
			if (velocityComponent == null)
			{
				GD.PrintErr("VelocityComponent node not found in Player");
				return;
			}
			if (rollState == null)
			{
				GD.PrintErr("Roll State node not found in Player");
				return;
			}
			healthComponent.Damage += TrackDamage;
		}

		private void TrackDamage(Attack attack)
		{
			Stats.data.DamageTaken += attack.FinalDamage;
			if (healthComponent.currentHealth <= 0)
			{
				if (!DeathGuard)
				{
					PlayerDead?.Invoke();
				}
				DeathGuard = true;
			}
		}
	}
}
