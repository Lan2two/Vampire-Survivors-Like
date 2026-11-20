using Godot;
using System;

public partial class Attack : States
{
    public override void Enter()
    {
        this.GetPlayer().anim.Play("attack");
        //on animationtimeout return to idle
        this.GetPlayer().anim.AnimationFinished += OnAnimationFinished;
    }
    public override void Exit()
    {
        this.GetPlayer().anim.AnimationFinished -= OnAnimationFinished;
    }
    private void OnAnimationFinished()
    {
        ChangeState("Idle");
    }
}