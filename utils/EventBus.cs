namespace SurvivorsGame
{
    using System;
    using Godot;

    public partial class EventBus
    {
        public static event Action AddButton;

        public static void InvokeAddButton()
        {
            AddButton?.Invoke();
            GD.Print("Add buttons invoked");
        }
    }
}