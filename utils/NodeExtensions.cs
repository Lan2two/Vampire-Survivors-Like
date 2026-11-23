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

        public static string GetActionKeyName(this Node node, string actionName)
        {
            Godot.Collections.Array<InputEvent> events = InputMap.ActionGetEvents(actionName);
            if (events.Count > 0)
            {
                return events[0].AsText().Replace(" - Physical", "").Capitalize();
            }
            else
            {
                return "";
            }
        }
    }
}
