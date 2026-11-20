using Godot;
using System;

public partial class EnemyIdle : States
{
    [Export] Enemy enemy;
    public override void Enter()
    {
        enemy.anim.Play("idle");
    }
}
