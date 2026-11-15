namespace SurvivorsGame
{
    using Godot;
    using System;

    [GlobalClass]
    public partial class HealthComponent : Node
    {
        [Export] public float maxHealth = 100f;
        [Export] public float Defense = 100f;

        public event Action<Attack> FinalDamageEvent;
        public event Action<Attack> InitalDamageEvent;
        public float currentHealth;
        private int K = 100;

        public override void _Ready()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(Attack attackData)
        {
            InitalDamageEvent?.Invoke(attackData);
            attackData.Damage *= K / (K + Defense);
            currentHealth -= attackData.Damage;
            FinalDamageEvent?.Invoke(attackData);
        }


    }
}
