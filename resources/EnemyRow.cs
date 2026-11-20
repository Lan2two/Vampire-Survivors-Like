namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    [GlobalClass]
    public partial class EnemyRow : Resource
    {
        [Export] public Array<EnemyData> Enemies { get; set; } = new();
    }
}
