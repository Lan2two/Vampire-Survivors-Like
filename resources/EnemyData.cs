namespace SurvivorsGame
{
    using Godot;
    [GlobalClass]
    public partial class EnemyData : Resource
    {
        [Export] public float Health { get; set; } = 15;
        [Export] public float Defense { get; set; } = 0f;
        [Export] public float Speed { get; set; } = 25f;
        [Export] public float CoinAmount { get; set; } = 2f;
        [Export] public float XpAmount { get; set; } = 2f;
        [Export] public Color Modulate { get; set; } = Colors.White;
        private PackedScene _enemyScene;
        [Export]
        public PackedScene EnemyScene
        {
            get => _enemyScene ??= ResourceLoader.Load<PackedScene>("res://scenes/enemy.tscn");
            set => _enemyScene = value;
        }
    }
}