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
		public UpgradeManager upgradeManager;
		public HealthComponent healthComponent;
		public VelocityComponent velocityComponent;
		public Roll rollState;
		public event Action PlayerDead;
		private bool DeathGuard = false;
		private float _baseMaxHealth = 100f;
		private float _baseMoveSpeed = 100f;
		public override void _Ready()
		{
			animatedSprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
			animationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
			inventoryManager = GetNodeOrNull<InventoryManager>("InventoryManager");
			upgradeManager = GetNodeOrNull<UpgradeManager>("UpgradeManager");
			healthComponent = GetNodeOrNull<HealthComponent>("HealthComponent");
			velocityComponent = GetNodeOrNull<VelocityComponent>("VelocityComponent");
			rollState = GetNodeOrNull<Roll>("StateMachine/roll");
			if (animatedSprite == null)
			{
				GD.PrintErr("AnimatedSprite2D node not found in Player");
			}
			if (animationPlayer == null)
			{
				GD.PrintErr("AnimationPlayer node not found in Player");
			}
			if (inventoryManager == null)
			{
				GD.PrintErr("InventoryManager node not found in Player");
			}
			if (upgradeManager == null)
			{
				GD.PrintErr("UpgradeManager node not found in Player");
			}
			if (healthComponent == null)
			{
				GD.PrintErr("HealthComponent node not found in Player");
			}
			if (velocityComponent == null)
			{
				GD.PrintErr("VelocityComponent node not found in Player");
			}
			if (rollState == null)
			{
				GD.PrintErr("Roll State node not found in Player");
			}

			_baseMaxHealth = healthComponent.maxHealth;
			_baseMoveSpeed = velocityComponent.maxSpeed;
			healthComponent.Damage += TrackDamage;
			EventBus.PlayerUpgrade += CalculateStats;
		}

		public override void _ExitTree()
		{
			if (healthComponent != null)
			{
				healthComponent.Damage -= TrackDamage;
			}
			if (inventoryManager != null)
			{
				EventBus.PlayerUpgrade -= CalculateStats;
			}
		}
		private void CalculateStats()
		{
			PlayerStatsData stats = new PlayerStatsData
			{
				MaxHealth = _baseMaxHealth,
				MoveSpeed = _baseMoveSpeed
			};
			upgradeManager.ApplyUpgrades(stats, UpgradeType.Player);
			float previousMaxhealth = healthComponent.maxHealth;
			healthComponent.maxHealth = stats.MaxHealth;
			GD.Print($"MaxHp: {(int)stats.MaxHealth} Speed: {(int)stats.MoveSpeed}");
			if (healthComponent.currentHealth > 0)
			{
				float heal = Mathf.Max(0f, stats.MaxHealth - previousMaxhealth);
				healthComponent.currentHealth = Mathf.Clamp(healthComponent.currentHealth + heal, 0f, stats.MaxHealth);
			}
			velocityComponent.maxSpeed = stats.MoveSpeed;
		}

		private void TrackDamage(Attack attack)
		{
			if (attack.Damage > 0)
			{
				Stats.data.DamageTaken += attack.Damage;
			}
			if (attack.Damage < 0)
			{
				Stats.data.HealthHealed -= attack.Damage;
			}
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
