using Godot;
using System;

public partial class Attack : States
{
    [Export] Player player;
    public override void Enter()
    {
        player.anim.Play("attack");
        //on animationtimeout return to idle
        player.anim.AnimationFinished += OnAnimationFinished;
    }
    public override void Exit()
    {
        player.anim.AnimationFinished -= OnAnimationFinished;
    }
    private void OnAnimationFinished()
    {
        ChangeState("Idle");
    }
}