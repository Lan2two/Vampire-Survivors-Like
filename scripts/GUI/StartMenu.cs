using Godot;
using System;

public partial class StartMenu : Control
{
    private VBoxContainer _menuContainer;
    private Button _startButton;
    private Button _exitButton;

    public override void _Ready()
    {
        _menuContainer = GetNodeOrNull<VBoxContainer>("MenuContainer");
        _startButton = GetNodeOrNull<Button>("MenuContainer/StartButton");
        _exitButton = GetNodeOrNull<Button>("MenuContainer/ExitButton");
        if (_menuContainer == null)
        {
            GD.PrintErr("MenuContainer not found in StartMenu");
            return;
        }
        if (_startButton == null)
        {
            GD.PrintErr("StartButton not found in StartMenu");
            return;
        }
        if (_exitButton == null)
        {
            GD.PrintErr("ExitButton not found in StartMenu");
            return;
        }
        _startButton.Pressed += StartButton;
        _exitButton.Pressed += ExitButton;
    }

    public override void _ExitTree()
    {
        if (_startButton != null)
        {
            _startButton.Pressed -= StartButton;
        }
        if (_exitButton != null)
        {
            _exitButton.Pressed -= ExitButton;
        }
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
