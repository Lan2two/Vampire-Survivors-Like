namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    [GlobalClass]
    public partial class Moratorium : StatusEffect
    {
        private PackedScene DisplayScene = GD.Load<PackedScene>("res://scenes/Status_Display.tscn");
        [Export] private float moratoriumMultiplier { get; set; } = 1.5f;
        public override string StatusName { get; set; } = "Moratorium";

        private const float _explosionTimerOffsets = 0.01f;
        private Array<float> StoredDamage = new();
        private StatusDisplay statusDisplay;
        private bool triggered;

        private HealthComponent _healthComponent;

        private void StoreDamage(Attack attack)
        {
            if (triggered) return;
            StoredDamage.Add(attack.Damage);
            attack.Damage = 0;
        }

        public override void OnInflict(Node node)
        {
            if (!IsInstanceValid(node)) return;
            _healthComponent = node.GetComponent<HealthComponent>();
            if (DisplayScene == null)
            {
                GD.PrintErr("Display Scene unset");
            }
            statusDisplay = DisplayScene.Instantiate<StatusDisplay>();
            if (statusDisplay == null || statusDisplay is not StatusDisplay) return;
            Callable.From(() =>
            {
                node.AddChild(statusDisplay);
                statusDisplay.SetUp(this, node);
            }).CallDeferred();

            if (_healthComponent != null)
            {
                _healthComponent.InitalDamageEvent += StoreDamage;
            }
        }

        public override void OnExpire(Node node)
        {
            if (!IsInstanceValid(node)) return;
            triggered = true;
            for (int i = 0; i < StoredDamage.Count; i++)
            {
                if (!IsInstanceValid(this) || !IsInstanceValid(_healthComponent)) return;
                float damage = StoredDamage[i] * moratoriumMultiplier;
                node.GetTree().CreateTimer(_explosionTimerOffsets * i).Timeout += () =>
                {
                    if (!IsInstanceValid(_healthComponent)) return;
                    Attack attack = new() { Damage = damage, Crit = true };
                    _healthComponent.TakeDamage(attack);
                };
            }
            statusDisplay.QueueFree();
            if (_healthComponent != null)
            {
                _healthComponent.InitalDamageEvent -= StoreDamage;
            }
        }
    }
}
