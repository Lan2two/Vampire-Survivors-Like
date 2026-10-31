namespace SurvivorsGame
{
    using Godot;
    using System.Collections.Generic;

    public partial class MenuContainer : Control
    {
        private Dictionary<string, Button> buttons = new();
        private Dictionary<ulong, Tween> activeTweens = new();

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
                    button.PivotOffsetRatio = new Vector2(0.5f, 0.5f);
                    button.CustomButtonHovered += OnHover;
                    button.MouseExited += ResetScale;
                }
            }
        }

        private void OnHover(CustomButton button)
        {
            AnimateButtonScale(button, new Vector2(1.25f, 1.25f));

            foreach (Button child in buttons.Values)
            {
                if (child == button) { continue; }
                AnimateButtonScale(child, new Vector2(0.75f, 0.75f));
            }
        }

        private void ResetScale()
        {
            foreach (Button child in buttons.Values)
            {
                AnimateButtonScale(child, new Vector2(1f, 1f));
            }
        }

        private void AnimateButtonScale(Button target, Vector2 targetScale)
        {
            if (target == null || !IsInstanceValid(target))
            {
                return;
            }
            ulong buttonId = target.GetInstanceId();
            if (activeTweens.TryGetValue(buttonId, out Tween existingTween))
            {
                if (IsInstanceValid(existingTween) && existingTween.IsRunning())
                {
                    existingTween.Kill();
                }
            }


            Tween tween = target.CreateTween();
            activeTweens[buttonId] = tween;
            Vector2 initialScale = target.Scale;
            tween.TweenMethod(
                Callable.From<Vector2>((currentScale) => target.Scale = currentScale),
                initialScale,
                targetScale,
                0.085f
            ).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
        }
    }
}
