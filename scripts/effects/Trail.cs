namespace SurvivorsGame
{
    using Godot;
    using System;
    using System.Linq;


    public partial class Trail : Line2D
    {
        [Export] bool Fade = true;
        [Export] float TrailDuration = .5f;
        [Export] int TrailPoints = 20;
        private float _StartingModulateA;
        private Node2D bullet;
        public override void _Ready()
        {
            bullet = GetParent() as Node2D;
            bullet.CallDeferred(Node.MethodName.RemoveChild, this);
            GetTree().CurrentScene.CallDeferred(Node.MethodName.AddChild, this);
            _StartingModulateA = SelfModulate.A;
        }

        public override void _Process(double delta)
        {
            if (Fade)
            {
                Color currentModulate = SelfModulate;
                currentModulate.A = Mathf.Max(0, currentModulate.A - (float)(_StartingModulateA / TrailDuration * delta));
                SelfModulate = currentModulate;
                if (SelfModulate.A <= 0)
                {
                    QueueFree();
                }
            }
            if (IsInstanceValid(bullet))
            {
                AddPoint(bullet.GlobalPosition);
                if (Points.Count() > TrailPoints)
                {
                    RemovePoint(0);
                }
            }

        }

    }
}