namespace SurvivorsGame
{
    using Godot;
    public partial class XpOrb : Absorbable
    {
        public float XPAmount = 1.0f;

        public override void SetAmount(float amount) => XPAmount = amount;

        protected override void OnPickup(Player player)
        {
            player.playerXP += XPAmount;
            Stats.data.totalXP += XPAmount;
        }
    }
}