namespace SurvivorsGame
{
    using Godot;
    public partial class HpOrb : Absorbable
    {
        [Export] public float HPAmount = 5f;

        protected override void OnPickup(Player player)
        {
            player.healthComponent.currentHealth += HPAmount;
            Stats.data.HealthHealed += HPAmount;
        }
    }
}