namespace SurvivorsGame
{
    using Godot;
    public partial class Weapon : Node2D
    {
        [Export] public AttackComponent attackComponent;
        public void UpdateDamage(Resource configInstance, AttackComponent attackComponent)
        {
            IDamageDealing config = configInstance as IDamageDealing;
            int Randint = GD.RandRange(0, 100);
            bool Crit = false;
            float FinalDamage = config.Damage;
            if (Randint < config.CritRate)
            {
                Crit = true;
                FinalDamage *= config.CritDamageMultiplier;
            }
            attackComponent.damage = FinalDamage;
            attackComponent.knockback = config.Knockback;
            attackComponent.crit = Crit;
        }
    }
}