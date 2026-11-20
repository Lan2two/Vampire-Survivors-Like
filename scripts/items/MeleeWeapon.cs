namespace SurvivorsGame
{
    using Godot;
    using System;


    public partial class MeleeWeapon : Weapon, IWeapon, IWeaponConfigurable
    {
        [Export] private bool SwingVariant;
        public MeleeWeaponBase Baseconfig;
        private AnimationPlayer animationPlayer;
        private UpgradeManager upgradeManager;
        public MeleeWeaponBase config;
        private double timer = 0;
        private bool swingForward = true;

        public override void _Ready()
        {
            animationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
            if (animationPlayer == null)
            {
                GD.PrintErr("AnimationPlayer not found in MeleeWeapon");
                return;
            }
            animationPlayer.AnimationFinished += OnAnimationFinished;
            animationPlayer.AnimationStarted += OnAnimationStarted;
            EventBus.MeleeUpgrade += OnUpgrade;
        }

        public override void _EnterTree()
        {
            CallDeferred(MethodName.DeferredReady);
        }

        public override void _ExitTree()
        {
            if (animationPlayer != null)
            {
                animationPlayer.AnimationFinished -= OnAnimationFinished;
                animationPlayer.AnimationStarted -= OnAnimationStarted;
            }
            EventBus.MeleeUpgrade -= OnUpgrade;
        }

        private void DeferredReady()
        {
            upgradeManager = this.GetPlayer().upgradeManager;
        }

        private void OnUpgrade()
        {
            config = (MeleeWeaponBase)Baseconfig.Duplicate();
            upgradeManager.ApplyUpgrades(config, UpgradeType.Melee);
            UpdateDamage(config, attackComponent);
        }
        public void Use()
        {
            if (timer > 0)
            {
                return;
            }
            OnUpgrade();

            Swing();
        }

        public void Configure(WeaponBase weaponData)
        {
            if (weaponData is not MeleeWeaponBase meleeWeaponData)
            {
                throw new ArgumentException("MeleeWeapon requires MeleeWeaponBase data.", nameof(weaponData));
            }

            Baseconfig = meleeWeaponData;
        }
        public override void _PhysicsProcess(double delta)
        {
            timer -= delta;
        }
        private void Swing()
        {
            if (timer > 0)
            {
                return;
            }
            float speedMultiplier = config.SwingSpeedMultiplier;
            timer = config.swingCooldown / speedMultiplier;
            animationPlayer.SpeedScale = speedMultiplier;
            //GD.Print($"Swinging weapon with speed multiplier: {(int)speedMultiplier}, cooldown: {(int)config.swingCooldown}, timer set to: {(int)timer}");

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
        private void OnAnimationStarted(StringName animName)
        {
            if (animName != "swing")
            {
                return;
            }
        }
    }
}
