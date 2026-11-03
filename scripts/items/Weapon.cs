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
            float FinalDamage = config.GetStat("Damage").CalculateFinal();
            if (Randint < config.GetStat("CritRate").CalculateFinal())
            {
                Crit = true;
                FinalDamage *= (config.GetStat("CritDamage").CalculateFinal() / 100) + 1;
            }
            attackComponent.damage = FinalDamage;
            attackComponent.knockback = config.GetStat("Knockback").CalculateFinal();
            attackComponent.crit = Crit;
        }
    }
}