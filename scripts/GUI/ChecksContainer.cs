namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    public partial class ChecksContainer : HBoxContainer
    {
        Array<CheckBox> CheckBoxes = new();
        public override void _Ready()
        {
        }

        public void AddChecks(int count)
        {
            int currentCount = 0;
            CheckBoxes.Clear();
            foreach (Node child in GetChildren())
            {
                child.QueueFree();
            }

            for (int i = currentCount; i < count; i++)
            {
                CheckBox checkBox = new()
                {
                    MouseFilter = MouseFilterEnum.Ignore
                };
                CheckBoxes.Add(checkBox);
                AddChild(checkBox);
            }
        }

        public void ToggleBox(int i, bool state = true)
        {
            GD.Print(i);
            GD.Print(CheckBoxes);
            CheckBoxes[i].ButtonPressed = state;
        }

        public void ToggleAllBoxes(bool state)
        {
            foreach (CheckBox box in CheckBoxes)
            {
                box.ButtonPressed = state;
            }
        }
    }
}