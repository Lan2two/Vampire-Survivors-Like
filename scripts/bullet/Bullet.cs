using Godot;
using System;

public partial class Bullet : Area2D
{
    [Export] int Damage = 10;
    public override void _Ready()
    {
        //connect the area entered signal to a function
        Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
    }
    private void OnAreaEntered(Area2D area)
    {
        if (area is HitboxComponent hitbox)
        {
            hitbox.TakeDamage(Damage);
            GD.Print("hit");
        }
    }
}
