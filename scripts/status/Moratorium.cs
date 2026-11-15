namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class Moratorium : Node2D
    {
        [Export] double duration { get; set; } = 10;
        private HealthComponent _healthComponent;
        private TextureProgressBar _progressBar;
        private float StoredDamage;
        private double timer;
        private bool triggered;
        public override void _Ready()
        {
            _healthComponent = GetNodeOrNull<HealthComponent>("../HealthComponent");
            _progressBar = GetNodeOrNull<TextureProgressBar>("Display");
            if (_healthComponent == null)
            {
                GD.PrintErr("Moratorium couldn't find healthComponent");
                return;
            }
            if (_progressBar == null)
            {
                GD.PrintErr("Moratorium couldn't find Display");
                return;
            }
            _progressBar.Step = 0.01;
            _progressBar.MaxValue = duration;
            _healthComponent.InitalDamageEvent += Effect;
        }

        public override void _ExitTree()
        {
            if (_healthComponent != null)
            {
                _healthComponent.InitalDamageEvent -= Effect;
            }
        }

        public override void _Process(double delta)
        {
            timer += delta;
            if (timer >= duration)
            {
                Trigger();
            }
            Vector2 offset = _progressBar.Size * _progressBar.Scale / 2;
            _progressBar.GlobalPosition = GlobalPosition - offset;
            _progressBar.Value = timer;
        }


        private void Effect(Attack attack)
        {
            if (triggered) return;
            StoredDamage += attack.Damage;
            attack.Damage = 0;
            GD.Print($"Stored Damage: {StoredDamage}");
        }

        private void Trigger()
        {
            triggered = true;
            Attack attack = new Attack
            {
                Damage = StoredDamage * 1.5f,
                Crit = true
            };
            _healthComponent.TakeDamage(attack);
            QueueFree();
        }
    }
}