namespace SurvivorsGame
{
    using Godot;
    using System;
    public partial class Coin : Absorbable
    {
        public int CoinAmount = 1;
        public static event Action CoinPickup;

        public override void SetAmount(float amount) => CoinAmount = (int)amount;

        protected override void OnPickup(Player player)
        {
            Stats.data.Coins += CoinAmount;
            CoinPickup?.Invoke();
        }

        public override float GetAmount() => CoinAmount;
    }
}