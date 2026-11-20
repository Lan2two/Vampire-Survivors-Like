
namespace SurvivorsGame
{
    using Godot;

    [GlobalClass]
    public partial class PlayerNodeStrategy : Strategy, IAttachment
    {
        [Export] PackedScene packedScene;

        public PlayerNodeStrategy()
        {
            upgradeType = UpgradeType.Player;
            ItemName = "Add Node";
            ItemDescription = "Adds Node to Player";
            MaxStackSize = 1;
        }
        public void ApplyUpgrade(Node2D node)
        {
            if (packedScene == null || node == null)
            {
                return;
            }
            Node2D sceneInstance = packedScene.Instantiate<Node2D>();
            node.AddChild(sceneInstance);
        }
    }
}