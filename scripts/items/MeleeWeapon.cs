namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class MeleeWeapon : Weapon, IWeapon, IWeaponConfigurable, IUpgradable
    {
        [Export] private bool SwingVariant;
        public MeleeWeaponBase Baseconfig;
        private AnimationPlayer animationPlayer;
        private UpgradeManager upgradeManager;
        private double timer = 0;
        private bool swingForward = true;
        public Resource currentConfig { get; set; }

        public override void _Ready()
        {
            animationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
            if (animationPlayer == null)
            {
                GD.PrintErr("AnimationPlayer not found in MeleeWeapon");
                return;
            }

            if (upgradeManager == null)
            {
                upgradeManager = this.GetPlayer()?.upgradeManager;
            }

            if (currentConfig == null && Baseconfig != null)
            {
                currentConfig = Baseconfig;
            }

            animationPlayer.AnimationFinished += OnAnimationFinished;
            EventBus.MeleeUpgrade += OnUpgrade;

            if (currentConfig != null && upgradeManager != null)
            {
                OnUpgrade();
            }
        }

        public override void _ExitTree()
        {
            if (IsInstanceValid(animationPlayer))
            {
                animationPlayer.AnimationFinished -= OnAnimationFinished;
            }
            EventBus.MeleeUpgrade -= OnUpgrade;
        }

        private void OnUpgrade()
        {
            if (upgradeManager == null)
            {
                upgradeManager = this.GetPlayer()?.upgradeManager;
            }

            if (upgradeManager != null && currentConfig != null)
            {
                upgradeManager.ApplyUpgrades(this, UpgradeType.Melee);
            }
        }

        public void Use()
        {
            if (timer > 0 || currentConfig is not MeleeWeaponBase activeConfig)
            {
                return;
            }

            UpdateDamage(activeConfig, attackComponent);
            Swing(activeConfig);
        }

        public void Configure(WeaponBase weaponData)
        {
            if (weaponData is not MeleeWeaponBase meleeWeaponData)
            {
                throw new ArgumentException("MeleeWeapon requires MeleeWeaponBase data.", nameof(weaponData));
            }

            Baseconfig = meleeWeaponData;
            currentConfig = meleeWeaponData;
            OnUpgrade();
        }

        public override void _PhysicsProcess(double delta)
        {
            timer -= delta;
        }

        private void Swing(MeleeWeaponBase activeConfig)
        {
            if (timer > 0 || animationPlayer == null)
            {
                return;
            }

            float speedMultiplier = activeConfig.GetStat("SwingSpeed").CalculateFinal();
            timer = activeConfig.GetStat("SwingCooldown").CalculateFinal() / speedMultiplier;
            animationPlayer.SpeedScale = speedMultiplier;

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
        }

        private void OnAnimationFinished(StringName animName)
        {
            if (animName != "swing")
            {
                return;
            }
            attackComponent.ClearHash();
        }
    }
}
