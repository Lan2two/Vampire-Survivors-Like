namespace SurvivorsGame
{
    using Godot;

    public partial class EnemySpawn : States
    {
        [Export] Enemy enemy;
        public override void Enter()
        {
            enemy.animatedSprite.Play("spawn");
            enemy.animatedSprite.AnimationFinished += OnAnimationFinished;
            enemy.collisionbox.SetDeferred("disabled", true);

        }
        public override void Exit()
        {
            enemy.animatedSprite.AnimationFinished -= OnAnimationFinished;
            enemy.collisionbox.SetDeferred("disabled", false);
        }

        private void OnAnimationFinished()
        {
            ChangeState("Chase");
        }
    }
}
