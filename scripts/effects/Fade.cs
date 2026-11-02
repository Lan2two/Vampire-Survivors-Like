namespace SurvivorsGame
{
    using Godot;

    public partial class Fade : ColorRect
    {

        public override void _Ready()
        {
            FadeIn(true);
        }

        private void FadeIn(bool fadeIn)
        {
            Visible = true;
            Tween tween = CreateTween();
            float startAlpha = fadeIn ? 1f : 0f;
            float endAlpha = fadeIn ? 0f : 1f;
            Color currentModulate = Modulate;
            currentModulate.A = startAlpha;
            Modulate = currentModulate;
            tween.TweenProperty(this, "modulate:a", endAlpha, .5f)
            .SetTrans(Tween.TransitionType.Linear);
            tween.Finished += () =>
            {
                if (fadeIn)
                {
                    Visible = false;
                }
                tween.Dispose();
            };
        }
    }
}