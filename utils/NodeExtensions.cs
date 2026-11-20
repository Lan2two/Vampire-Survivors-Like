using Godot;

public static class NodeExtensions
{
    public static Player GetPlayer(this Node node)
    {
        SceneTree tree = node.GetTree();
        Player player = tree.GetFirstNodeInGroup("player") as Player;
        if (player != null)
        {
            return player;
        }
        else
        {
            GD.PrintErr("Player node not found in 'player' group.");
            return null;
        }
    }
}