using Godot;
using System;

public partial class StartMenu : Control
{
    VBoxContainer _menuContainer;
    Button _startButton;
    Button _exitButton;
    public override void _Ready()
    {
        _menuContainer = GetNodeOrNull<VBoxContainer>("MenuContainer");
        _startButton = GetNodeOrNull<Button>("MenuContainer/StartButton");
        _exitButton = GetNodeOrNull<Button>("MenuContainer/ExitButton");

        _startButton.Pressed += StartButton;
        _exitButton.Pressed += ExitButton;
    }

    public override void _ExitTree()
    {
        _startButton.Pressed -= StartButton;
        _exitButton.Pressed -= ExitButton;
    }


    private void StartButton()
    {
        GetTree().ChangeSceneToFile("res://scenes/Game.tscn");
    }

    private void ExitButton()
    {
        GetTree().Quit();
    }
}
