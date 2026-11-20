using Godot;
using System;

[GlobalClass]
public partial class WanderComponent : Node
{
    [Export] CharacterBody2D characterBody;
    [Export] int WanderSpeed = 20;
    [Export] int WanderRadius = 100;
    private Vector2 randomDirection = Vector2.Zero;
    Vector2 targetPosition;
    Vector2 originPoint;

    public void getRandomDirection
}
