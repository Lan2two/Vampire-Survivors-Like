namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class VisibleOnScreenNotifier2d : VisibleOnScreenNotifier2D
    {
        [Export] private Node2D Parent;
        public override void _Ready()
        {
            ScreenExited += OnScreenExit;
        }

        private void OnScreenExit()
        {
            if (Parent is IPoolable poolable)
            {
                poolable.OnDespawn();
            }
        }

    }
}