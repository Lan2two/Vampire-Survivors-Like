using Godot;
using System;

public partial class EnemySpawn : States
{
    [Export] Enemy enemy;
    public override void Enter()
    {
        enemy.anim.Play("spawn");
        enemy.anim.AnimationFinished += OnAnimationFinished;
        enemy.collisionbox.SetDeferred("disabled", true);

    }
    public override void Exit()
    {
        enemy.anim.AnimationFinished -= OnAnimationFinished;
        enemy.collisionbox.SetDeferred("disabled", false);
    }

    private void OnAnimationFinished()
    {
        ChangeState("Idle");
    }
}
