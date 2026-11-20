namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class XpBar : ProgressBar
    {
        [Export] private double LevelIncrement = 3;
        [Export] private int AbsorbAllXpLevelInterval = 10;
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

        private void LevelUp()
        {
            _player.playerXP -= (float)MaxValue;
            Value = _player.playerXP;
            MaxValue += LevelIncrement;
            _currentLevel++;
            XpLabel.Text = $"Level: {_currentLevel}";
            GD.Print($"Value: {(int)Value}, MaxValue: {(int)MaxValue}, Level: {_currentLevel}");
            LevelUpEvent?.Invoke();
        }

    }
}