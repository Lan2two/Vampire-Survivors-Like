namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class XpBar : ProgressBar
    {
        public Label XpLabel;
        private Player player;
        public override void _Ready()
        {
            player = this.GetPlayer();
            XpLabel = GetNodeOrNull<Label>("XpLabel");
            if (XpLabel == null)
            {
                GD.PrintErr("XpLabel not found in XpBar");
                return;
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            Value = player.playerXP;
        }


    }
}