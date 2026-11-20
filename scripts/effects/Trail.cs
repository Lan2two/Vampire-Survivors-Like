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
        private Node2D parent;
        private SceneTree tree;
        public override void _Ready()
        {
            parent = GetParent() as Node2D;
            tree = GetTree();
            TopLevel = true;
            _StartingModulateA = SelfModulate.A;
            parent.TreeExiting += OnParentExiting;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
        }

        private void OnParentExiting()
        {
            parent?.RemoveChild(this);
            tree.CurrentScene.CallDeferred(Node.MethodName.AddChild, this);
            Fade = true;
        }

        public override void _Process(double delta)
        {

            if (Fade || !IsInstanceValid(parent))
            {
                Color currentModulate = SelfModulate;
                currentModulate.A = Mathf.Max(0, currentModulate.A - (float)(_StartingModulateA / TrailDuration * delta));
                SelfModulate = currentModulate;
                if (SelfModulate.A <= 0)
                {
                    QueueFree();
                }
            }
            if (IsInstanceValid(parent))
            {
                AddPoint(parent.GlobalPosition);
                if (Points.Count() > TrailPoints)
                {
                    RemovePoint(0);
                }
            }
        }
    }
}
