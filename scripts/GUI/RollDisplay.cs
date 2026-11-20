namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class RollDisplay : TextureProgressBar
    {

        private Roll roll;
        public override void _Ready()
        {
            roll = this.GetPlayer().rollState;
            if (roll == null)
            {
                SetProcess(false);
                return;
            }
            Visible = true;
            Step = 0.01;
            MaxValue = roll.rollCooldown;
        }

        public override void _Process(double delta)
        {
            if (roll == null)
            {
                return;
            }
            Value = roll.timer;
        }

    }
}