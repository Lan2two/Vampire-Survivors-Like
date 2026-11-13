namespace SurvivorsGame
{
    using Godot;

    public partial class ShopCoins : Label
    {
        public override void _Ready()
        {
            SaveSystem.LoadData();
        }

        public override void _PhysicsProcess(double delta)
        {
            SaveSystem.LoadData();
            Text = $"{SaveSystem.LoadData().TotalCoins}";
        }


    }
}