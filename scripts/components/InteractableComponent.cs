using System;
using Godot;

[GlobalClass]
public partial class InteractableComponent : Area2D
{
    [Export] public string interactName = "";
    [Export] public bool isInteractable = true;
    [Signal] public delegate void InteractEventHandler();
}