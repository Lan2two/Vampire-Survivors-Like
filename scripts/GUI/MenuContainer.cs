namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

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
                    button.PivotOffset = button.Size / 2;
                    button.CustomButtonHovered += OnHover;
                    button.MouseExited += ResetScale;
                }
            }
        }

        private void OnHover(CustomButton button)
        {
            Tween Hovertween = CreateTween();
            Hovertween.TweenProperty(button, "scale", new Vector2(1.25f, 1.25f), .085f);
            foreach (Button child in buttons.Values)
            {
                if (child == button)
                {
                    continue;
                }
                child.PivotOffset = child.Size / 2;
                Tween tween = CreateTween();
                tween.TweenProperty(child, "scale", new Vector2(.75f, .75f), .085f);
            }
        }

        private void ResetScale()
        {
            foreach (Button child in buttons.Values)
            {
                Tween Hovertween = CreateTween();
                Hovertween.TweenProperty(child, "scale", new Vector2(1f, 1f), .085f);
            }
        }

    }
}