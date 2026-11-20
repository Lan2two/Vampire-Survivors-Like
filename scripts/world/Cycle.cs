namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class Cycle : CanvasModulate
    {
        [Export] Gradient TimeGradient;
        [Export] public float CycleTimeSeconds = 60f;
        [Export] public float StartingTime = 0f;

        private double _currentSeconds;

        public override void _Ready()
        {
            if (TimeGradient == null)
            {
                GD.PrintErr("Gradient not set in Cycle");
            }
            _currentSeconds = StartingTime / 24.0f * CycleTimeSeconds;
        }

        public override void _Process(double delta)
        {
            if (TimeGradient == null) return;
            _currentSeconds += delta;
            _currentSeconds %= CycleTimeSeconds;
            Color = TimeGradient.Sample(GetTimePercent());
        }

        public float GetTimePercent()
        {
            return (float)(_currentSeconds / CycleTimeSeconds);
        }
    }
}