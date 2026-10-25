namespace SurvivorsGame
{
    using Godot;
    using System;
    using System.Linq;


    public partial class Trail : Line2D
    {
        private Node2D bullet;
        public override void _Ready()
        {
            bullet = GetParent() as Node2D;
            bullet.CallDeferred(Node.MethodName.RemoveChild, this);
            GetTree().CurrentScene.CallDeferred(Node.MethodName.AddChild, this);
        }

        public override void _Process(double delta)
        {
            Color currentModulate = SelfModulate;
            currentModulate.A = Mathf.Max(0, currentModulate.A - (float)(1.5f * delta));
            SelfModulate = currentModulate;
            if (SelfModulate.A <= 0)
            {
                QueueFree();
            }
            if (!IsInstanceValid(bullet))
            {
                return;
            }
            AddPoint(bullet.GlobalPosition);
            if (Points.Count() > 20)
            {
                RemovePoint(0);
            }
        }

    }
}