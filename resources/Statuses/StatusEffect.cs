namespace SurvivorsGame
{
    using Godot;
    [GlobalClass]
    public partial class StatusEffect : Resource
    {
        [Export] public double Duration { get; set; } = 5f;
        [Export] public float TickInterval { get; set; } = 0f;
        [Export] public bool IsStackable { get; set; } = false;
        public virtual string StatusName { get; set; }
        public double timer;
        public double tickTimer;

        public void Initialize()
        {
            timer = Duration;
            tickTimer = TickInterval;
        }

        public bool Update(double delta, Node target)
        {
            timer -= delta;

            if (TickInterval > 0)
            {
                tickTimer -= delta;
                if (tickTimer < -0)
                {
                    tickTimer += TickInterval;
                    OnTick(target);
                }
            }

            return timer <= 0f;
        }

        public virtual void OnInflict(Node target) { }
        public virtual void OnTick(Node target) { }
        public virtual void OnExpire(Node target) { }
    }
}