namespace SurvivorsGame
{
    using Godot;

    public partial class ShopCoins : Label
    {
        public override void _Ready()
        {
            SaveSystem.Load<GameSaveData>();
        }

        public override void _PhysicsProcess(double delta)
        {
            GameSaveData data = SaveSystem.Load<GameSaveData>();
            Text = $"{data.Coins}";
        }


    }
}