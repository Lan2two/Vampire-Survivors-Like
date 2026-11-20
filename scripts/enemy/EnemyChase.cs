namespace SurvivorsGame
{
    using Godot;
    using System;

    public partial class EnemyChase : States
    {
        [Export] Enemy enemy;
        [Export] PathfindComponent pathfindComponent;

        public override void Enter()
        {
            enemy.anim.Play("idle");
        }
        public override void PhysicsUpdate(double delta)
        {
            pathfindComponent?.PathfindToPlayer();
        }

    }
}
