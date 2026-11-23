namespace SurvivorsGame
{
    using Godot;
    public partial class HpOrb : Absorbable
    {
        [Export] public float HPAmount = 5f;
        public override void SetAmount(float amount) => HPAmount = amount;

        protected override void OnPickup(Player player)
        {
            player.healthComponent.currentHealth += HPAmount;
            Stats.data.HealthHealed += HPAmount;
        }

        public override float GetAmount() => HPAmount;

    }
}