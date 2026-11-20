namespace SurvivorsGame
{
    using Godot;

    public partial class FloatingNumbers : Node2D
    {
        private const double FadeDelay = 0.3;
        private Node damageLabel;
        private Vector2 startPosition;
        private Vector2 targetPosition;
        private double elapsed;
        private double movementDuration;
        private double fadeDuration;
        private bool animationStarted;

        public void DisplayDamage(Attack attackData, double targetDuration, double duration)
        {
            if (GetChildCount() == 0)
            {
                GD.PrintErr("FloatingNumbers scene is missing its DamageLabel child.");
                QueueFree();
                return;
            }

            damageLabel = GetChild(0);
            damageLabel.SetDeferred("text", FormatDamage(attackData.Damage));
            startPosition = Position;
            targetPosition = startPosition + new Vector2(GD.RandRange(-30, 30), -35);
            movementDuration = targetDuration;
            fadeDuration = duration;
            animationStarted = true;
        }

        public override void _Process(double delta)
        {
            if (!animationStarted)
            {
                return;
            }

            elapsed += delta;

            float movementProgress = movementDuration <= 0
                ? 1f
                : Mathf.Clamp((float)(elapsed / movementDuration), 0f, 1f);
            float easedMovement = 1f - Mathf.Pow(1f - movementProgress, 2f);
            Position = startPosition.Lerp(targetPosition, easedMovement);

            float fadeProgress = fadeDuration <= 0
                ? 1f
                : Mathf.Clamp((float)((elapsed - FadeDelay) / fadeDuration), 0f, 1f);
            Color color = Modulate;
            Modulate = new Color(color.R, color.G, color.B, 1f - fadeProgress);

            if (elapsed >= System.Math.Max(movementDuration, FadeDelay + fadeDuration))
            {
                QueueFree();
            }
        }

        private static string FormatDamage(float damage)
        {
            int roundedDamage = Mathf.RoundToInt(damage);
            bool isNegative = roundedDamage < 0;
            uint remaining = isNegative ? (uint)(-(long)roundedDamage) : (uint)roundedDamage;
            char[] characters = new char[11];
            int index = characters.Length;

            do
            {
                characters[--index] = (char)('0' + remaining % 10);
                remaining /= 10;
            }
            while (remaining > 0);

            if (isNegative)
            {
                characters[--index] = '-';
            }

            return new string(characters, index, characters.Length - index);
        }
    }
}
