namespace SurvivorsGame
{
    using Godot;
    public partial class XpOrb : Absorbable
    {
        public float XPAmount = 1.0f;
        public override void _Ready()
        {
            base._Ready();
            BodyEntered += OnBodyEntered;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            BodyEntered -= OnBodyEntered;
        }

        public void SetXPAmount(float amount)
        {
            XPAmount = amount;
        }

        private void OnBodyEntered(Node body)
        {
            if (body is Player)
            {
                Player player = (Player)body;
                player.playerXP += XPAmount;
                Stats.data.totalXP += XPAmount;
            }
        }
    }
}