namespace SurvivorsGame
{
    using Godot;
    public partial class Coin : Absorbable
    {
        public int CoinAmount = 1;
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

        public override void SetAmount(float amount)
        {
            CoinAmount = (int)amount;
        }

        private void OnBodyEntered(Node body)
        {
            if (body is Player)
            {
                Stats.data.Coins += CoinAmount;
            }
        }
    }
}