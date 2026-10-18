namespace SurvivorsGame
{
    using System.Collections.Generic;
    using Godot;
    using Godot.Collections;
    [GlobalClass]
    public partial class CompositeStrategy : Strategy
    {
        [Export] public Array<Strategy> Children { get; set; } = new();

        public CompositeStrategy()
        {
            ItemName = "Combo Upgrade";
        }

        public override string GetDescription()
        {
            if (!string.IsNullOrEmpty(ItemDescription))
            {
                return ItemDescription;
            }

            List<string> lines = new();
            foreach (Strategy child in Children)
            {
                if (child == null)
                {
                    GD.PrintErr("Child Null");
                    continue;
                }
                lines.Add(child.GetDescription());
            }
            return string.Join("\n", lines);
        }
    }
}