namespace SurvivorsGame
{
    using Godot;

    public partial class Fade : ColorRect
    {
        [Export] bool FadeInOnReady = true;
        private Tween tween;

        public override void _Ready()
        {
            if (FadeInOnReady)
            {
                FadeIn(true);
            }
        }

        public void FadeIn(bool fadeIn)
        {
            Visible = true;
            if (tween != null && IsInstanceValid(tween))
            {
                tween.Kill();
            }

            tween = CreateTween();
            float startAlpha = fadeIn ? 1f : 0f;
            float endAlpha = fadeIn ? 0f : 1f;

            Color currentModulate = Modulate;
            currentModulate.A = startAlpha;
            Modulate = currentModulate;
            tween.TweenMethod(
                Callable.From<float>(alpha => Modulate = new Color(Modulate.R, Modulate.G, Modulate.B, alpha)),
                startAlpha,
                endAlpha,
                0.5f
            ).SetTrans(Tween.TransitionType.Linear);

            tween.Finished += () =>
            {
                if (fadeIn)
                {
                    Visible = false;
                }
            };
        }
    }
}
