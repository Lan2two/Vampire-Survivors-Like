namespace SurvivorsGame
{
    using Godot;

    public partial class Pause : Label
    {
        private bool Paused = false;
        public override void _Input(InputEvent @event)
        {
            if (Input.IsActionJustPressed("pause"))
            {
                Paused = !Paused;
                if (Paused)
                {
                    PauseGame(true);
                }
                else
                {
                    PauseGame(false);
                }
            }
        }
        private void PauseGame(bool set)
        {
            GetTree().Paused = set;
            Visible = set;
            if (set)
                ProcessMode = ProcessModeEnum.Always;
            else
                ProcessMode = ProcessModeEnum.Pausable;
        }
    }
}