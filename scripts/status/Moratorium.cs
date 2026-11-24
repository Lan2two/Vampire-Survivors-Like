namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    public partial class Moratorium : Node2D
    {
        [Export] private double duration { get; set; } = 10;
        [Export] private float moratoriumMultiplier { get; set; } = 1.5f;
        private const float _explosionOffsets = 0.01f;
        private Array<float> StoredDamage = new();
        private HealthComponent _healthComponent;
        private TextureProgressBar _progressBar;
        private AnimatedSprite2D _animatedSprite;
        private Vector2 _progressbarOffset;
        private double timer;
        private bool triggered;

        public override void _Ready()
        {
            _healthComponent = GetNodeOrNull<HealthComponent>("../HealthComponent");
            _progressBar = GetNodeOrNull<TextureProgressBar>("Display");
            _animatedSprite = GetNodeOrNull<AnimatedSprite2D>("../AnimatedSprite2D");

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

            GlobalScale = Vector2.One;

            UpdateProgressBarScale();

            _progressBar.Step = 0.01;
            _progressBar.MaxValue = duration;
            _healthComponent.InitalDamageEvent += Effect;
        }

        private void UpdateProgressBarScale()
        {
            if (_animatedSprite == null || _progressBar == null) return;

            Texture2D texture = _animatedSprite.SpriteFrames.GetFrameTexture(_animatedSprite.Animation, _animatedSprite.Frame);
            Vector2 parentWorldSize = texture.GetSize() * _animatedSprite.GlobalScale;

            _progressBar.Scale = parentWorldSize / _progressBar.Size;
            _progressbarOffset = _progressBar.Size * _progressBar.Scale / 2f;
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

            if (GlobalScale != Vector2.One)
            {
                GlobalScale = Vector2.One;
                UpdateProgressBarScale();
            }

            _progressBar.GlobalPosition = GlobalPosition - _progressbarOffset;
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
                if (!IsInstanceValid(this) || !IsInstanceValid(_healthComponent)) return;
                float damage = StoredDamage[i] * moratoriumMultiplier;
                GetTree().CreateTimer(_explosionOffsets * i).Timeout += () =>
                {
                    if (!IsInstanceValid(_healthComponent)) return;
                    Attack attack = new() { Damage = damage, Crit = true };
                    _healthComponent.TakeDamage(attack);
                };
            }
            QueueFree();
        }
    }
}
