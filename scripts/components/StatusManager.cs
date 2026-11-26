namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;
    [GlobalClass]
    public partial class StatusManager : Node
    {
        [Export] private Node _root;
        [Export] private Array<StatusEffect> StartingStatuses = new();
        public readonly Array<StatusEffect> _activeStatuses = new();

        public override void _Ready()
        {
            SetProcess(false);
            if (StartingStatuses.Count != 0)
            {
                foreach (StatusEffect status in StartingStatuses)
                {
                    ApplyStatus(status);
                }
            }
        }

        public override void _Process(double delta)
        {
            foreach (StatusEffect status in _activeStatuses)
            {
                if (status.Update(delta, _root))
                {
                    RemoveStatus(status);
                }
            }
        }

        public void ApplyStatus(StatusEffect status)
        {
            if (status == null || _root == null) return;
            if (!status.IsStackable && _activeStatuses.Contains(status)) return;
            StatusEffect instance = (StatusEffect)status.Duplicate();
            instance.Initialize();

            _activeStatuses.Add(instance);
            SetProcess(true);

            instance.OnInflict(_root);
        }

        public void RemoveStatus(StatusEffect status)
        {
            if (_activeStatuses.Remove(status))
            {
                status.OnExpire(_root);
                if (_activeStatuses.Count == 0)
                {
                    SetProcess(false);
                }
            }
        }
    }
}