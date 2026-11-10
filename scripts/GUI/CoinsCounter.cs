namespace SurvivorsGame
{
    using Godot;

    public partial class CoinsCounter : Label
    {
        public override void _Ready()
        {
            Text = $"Coins: {Stats.data.Coins}";
            Coin.CoinPickup += UpdateCounter;
        }

        public override void _ExitTree()
        {
            Coin.CoinPickup += UpdateCounter;
        }

        private void UpdateCounter()
        {
            Text = $"Coins: {Stats.data.Coins}";
        }


    }
}
