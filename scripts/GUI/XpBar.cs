namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;
    using System;

    public partial class XpBar : ProgressBar
    {
        [Export] private Array<double> LevelIncrement = new() { 3, 5 };
        [Export] private int IncrementIncrease = 10;
        public Label XpLabel;
        private int _currentLevel = 0;
        private Player _player;
        private int _level = 0;
        private int _increment;

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
            _increment = IncrementIncrease;
            MaxValue = LevelIncrement[0];
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
            _currentLevel++;
            if (_level < LevelIncrement.Count - 1 && _currentLevel >= _increment)
            {
                _increment += IncrementIncrease;
                _level++;
            }
            _player.playerXP -= (float)MaxValue;
            Value = _player.playerXP;
            MaxValue += LevelIncrement[_level];
            //GD.Print(LevelIncrement[_level]);
            Stats.data.Level++;
            XpLabel.Text = $"Lvl {_currentLevel}";
            LevelUpEvent?.Invoke();
        }

    }
}