namespace SurvivorsGame
{
    using Godot;

    public partial class GameTime : Control
    {
        private Label _timeLabel;
        private Timer _gameTimer;

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

        public override void _ExitTree()
        {
            if (_gameTimer != null)
            {
                _gameTimer.Timeout -= OnTimeOut;
            }
        }


        private void OnTimeOut()
        {
            Stats.data.TotalSeconds++;
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            int _minutes = Stats.data.TotalSeconds / 60;
            int _seconds = Stats.data.TotalSeconds % 60;

            _timeLabel.Text = $"{_minutes:D2}:{_seconds:D2}";
        }

    }
}