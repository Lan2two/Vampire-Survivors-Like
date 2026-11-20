namespace SurvivorsGame
{
    using Godot;

    public partial class GameTime : Control
    {
        private Label _timeLabel;
        private Timer _gameTimer;
        private int _totalSeconds;

        public override void _Ready()
        {
            _timeLabel = GetNodeOrNull<Label>("TimeLabel");
            _gameTimer = GetNodeOrNull<Timer>("GameTimer");
            if (_timeLabel == null)
            {
                GD.PrintErr("TimeLabel not found in Timer");
                return;
            }
            if (_gameTimer == null)
            {
                GD.PrintErr("GameTimer not found in Timer");
                return;
            }
            _gameTimer.Timeout += OnTimeOut;
            UpdateLabel();
        }

        private void OnTimeOut()
        {
            _totalSeconds++;
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            int _minutes = _totalSeconds / 60;
            int _seconds = _totalSeconds % 60;

            _timeLabel.Text = $"{_minutes:D2}:{_seconds:D2}";
        }

    }
}