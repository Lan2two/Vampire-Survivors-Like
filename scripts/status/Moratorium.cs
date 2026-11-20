namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    public partial class Moratorium : Node2D
    {
        [Export] double duration { get; set; } = 10;
        private Array<float> StoredDamage = new();
        private HealthComponent _healthComponent;
        private TextureProgressBar _progressBar;
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
            StoredDamage.Add(attack.Damage);
            attack.Damage = 0;
        }

        private void Trigger()
        {
            triggered = true;
            for (int i = 0; i < StoredDamage.Count; i++)
            {
                float damage = StoredDamage[i] * 1.5f;
                GetTree().CreateTimer(0.01 * i).Timeout += () =>
                {
                    Attack attack = new()
                    {
                        Damage = damage,
                        Crit = true
                    };
                    _healthComponent.TakeDamage(attack);
                };
            }
            QueueFree();
        }
    }
}