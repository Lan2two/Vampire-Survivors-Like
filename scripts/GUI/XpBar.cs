namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class XpBar : ProgressBar
    {
        [Export] private double LevelIncrement = 3;
        public Label XpLabel;
        private int _currentLevel = 0;
        private Player _player;

        public event Action LevelUpEvent;
        public override void _Ready()
        {
            _player = this.GetPlayer();
            XpLabel = GetNodeOrNull<Label>("XpLabel");
            if (XpLabel == null)
            {
                GD.PrintErr("XpLabel not found in XpBar");
                return;
            }
            MaxValue = LevelIncrement;
        }

        public override void _PhysicsProcess(double delta)
        {
            Value = _player.playerXP;
            if (Value >= MaxValue)
            {
                LevelUp();
            }
        }

        public int GetLevel()
        {
            return _currentLevel;
        }

        private void LevelUp()
        {
            _player.playerXP -= (float)MaxValue;
            Value = _player.playerXP;
            MaxValue += LevelIncrement;
            _currentLevel++;
            Stats.data.Level++;
            XpLabel.Text = $"Level: {_currentLevel}";
            LevelUpEvent?.Invoke();
        }

    }
}