namespace SurvivorsGame
{
    using Godot;
    using System.Collections.Generic;

    public partial class MenuContainer : Control
    {
        [Export] private Vector2 offset = new Vector2(80, 130);
        private Dictionary<string, CustomButton> buttons = new();

        public override void _Ready()
        {
            SetupButtons();
        }

        private void SetupButtons()
        {
            foreach (Node child in GetChildren())
            {
                if (child is CustomButton button)
                {
                    string buttonName = button.Name.ToString();
                    if (buttons.ContainsKey(buttonName)) { continue; }

                    buttons.Add(buttonName, button);
                    button.CustomButtonHovered += OnHover;
                    button.MouseExited += ResetScale;

                    var keysList = new List<string>(buttons.Keys);
                    int buttonIndex = keysList.IndexOf(buttonName);
                    Vector2 StartingPosition = buttons[keysList[0]].Position;

                    for (int i = 1; i < keysList.Count; i++)
                    {
                        buttons[keysList[i]].Position = StartingPosition + offset * i;
                    }
                }
            }

        }

        private void OnHover(CustomButton button)
        {
            button.AnimateButtonScale(new Vector2(1.25f, 1.25f));

            foreach (CustomButton child in buttons.Values)
            {
                if (child == button) { continue; }
                child.AnimateButtonScale(new Vector2(0.75f, 0.75f));
            }
        }

        private void ResetScale()
        {
            foreach (CustomButton child in buttons.Values)
            {
                child.AnimateButtonScale(new Vector2(1f, 1f), .12f);
            }
        }
    }
}
