namespace SurvivorsGame
{
    using Godot;
    using System;
    [GlobalClass]
    public partial class CustomButton : Button
    {
        public event Action<CustomButton> CustomButtonPressed;
        public event Action<CustomButton> CustomButtonHovered;
        private Tween tween;

        public override void _Ready()
        {
            Pressed += OnButtonPressed;
            MouseEntered += OnMouseEntered;
        }

        private void OnButtonPressed()
        {
            CustomButtonPressed?.Invoke(this);
        }

        private void OnMouseEntered()
        {
            CustomButtonHovered?.Invoke(this);
        }

        public void AnimateButtonScale(Vector2 targetScale, double duration = 0.085f)
        {
            if (tween != null)
            {
                tween.Kill();
                tween = null;
            }
            tween = CreateTween();
            Vector2 initialScale = Scale;
            tween.TweenMethod(
                Callable.From<Vector2>((currentScale) => Scale = currentScale),
                initialScale,
                targetScale,
                duration
            ).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
            tween.Finished += () =>
            {
                if (tween != null)
                {
                    tween.Dispose();
                    tween = null;
                }
            };
        }
    }
}