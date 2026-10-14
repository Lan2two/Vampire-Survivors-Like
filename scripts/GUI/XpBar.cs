namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class XpBar : ProgressBar
    {
        [Export] private double LevelIncrement = 3;
        public Label XpLabel;
        private int CurrentLevel = 0;
        private Player player;

        public event Action LevelUpEvent;
        public override void _Ready()
        {
            player = this.GetPlayer();
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
            Value = player.playerXP;
            if (Value >= MaxValue)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            player.playerXP -= (float)MaxValue;
            MaxValue += LevelIncrement;
            CurrentLevel++;
            XpLabel.Text = $"Level: {CurrentLevel}";
            GD.Print($"Value: {(int)Value}, MaxValue: {(int)MaxValue}, Level: {CurrentLevel}");
            LevelUpEvent?.Invoke();
        }

    }
}