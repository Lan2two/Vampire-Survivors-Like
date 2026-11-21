namespace SurvivorsGame
{
    using Godot;

    public static class NodeExtensions
    {
        public static Player GetPlayer(this Node node)
        {
            for (Node current = node; GodotObject.IsInstanceValid(current); current = current.GetParent())
            {
                if (current is Player player)
                {
                    return player;
                }
            }

            if (!GodotObject.IsInstanceValid(node) || !node.IsInsideTree())
            {
                return null;
            }

            SceneTree tree = node.GetTree();
            return tree.GetFirstNodeInGroup("player") as Player;
        }
    }
}
