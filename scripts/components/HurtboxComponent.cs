namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class HurtboxComponent : Area2D
    {
        [Export] public HealthComponent healthComponent;

        public void TakeDamage(Attack attackData)
        {
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(attackData);
            }
        }
    }

}
