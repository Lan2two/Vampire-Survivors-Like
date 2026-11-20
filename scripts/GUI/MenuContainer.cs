namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;
    using System;

    public partial class MenuContainer : Control
    {
        private Dictionary<string, Button> buttons = new();
        public override void _Ready()
        {
            foreach (Node child in GetChildren())
            {
                if (child is CustomButton button)
                {
                    string buttonName = button.Name.ToString();
                    if (buttons.ContainsKey(buttonName))
                    {
                        continue;
                    }
                    buttons.Add(buttonName, button);
                    button.CustomButtonHovered += OnHover;
                    button.MouseExited += ResetScale;
                }
            }
        }

        private void OnHover(CustomButton button)
        {
            button.PivotOffset = button.Size / 2;
            Tween Hovertween = CreateTween();
            Hovertween.TweenProperty(button, "scale", new Vector2(1.2f, 1.2f), .05f);
            foreach (Button child in buttons.Values)
            {
                if (child == button)
                {
                    continue;
                }
                child.PivotOffset = child.Size / 2;
                Tween tween = CreateTween();
                tween.TweenProperty(child, "scale", new Vector2(.8f, .8f), .05f);
                child.Scale = new Vector2(.8f, .8f);
            }
        }

        private void ResetScale()
        {
            foreach (Button child in buttons.Values)
            {
                child.PivotOffset = child.Size / 2;
                Tween Hovertween = CreateTween();
                Hovertween.TweenProperty(child, "scale", new Vector2(1f, 1f), .05f);
            }
        }

    }
}