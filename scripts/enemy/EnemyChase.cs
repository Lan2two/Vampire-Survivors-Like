namespace SurvivorsGame
{
    using Godot;

    public partial class EnemyChase : States
    {
        [Export] Enemy enemy;
        [Export] PathfindComponent pathfindComponent;

        public override void Enter()
        {
            enemy.animatedSprite.Play("idle");
        }
        public override void PhysicsUpdate(double delta)
        {
            pathfindComponent?.PathfindToPlayer();
        }

    }
}
