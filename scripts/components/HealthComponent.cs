namespace SurvivorsGame
{
    using Godot;
    using System;

    [GlobalClass]
    public partial class HealthComponent : Node
    {
        [Export] public float maxHealth = 100f;

        public event Action<Attack> Damage;
        public float currentHealth;

        public override void _Ready()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(Attack attackData)
        {
            int Randint = GD.RandRange(0, 100);
            attackData.FinalDamage = attackData.BaseDamage;
            if (Randint < attackData.CritRate)
            {
                GD.Print("Crit");
                attackData.Crit = true;
                attackData.FinalDamage *= attackData.CritDamageMultiplier;
            }
            currentHealth -= attackData.FinalDamage;
            Damage?.Invoke(attackData);
        }


    }
}
