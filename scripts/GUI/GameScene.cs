

namespace SurvivorsGame
{
    using System;

    using Godot;
    using Godot.Collections;
    public partial class GameScene : Node2D
    {
        [Export] private int _absorbXpInterval = 100;
        private Control gameOverNode;
        private Label gameOverLabel;
        private CanvasLayer HUD;
        private Player player;
        private ColorRect Dim;
        private bool _isChangingScene = false;
        private int _currentInterval = 1;

        public static event Action GameOverEvent;

        public override void _Ready()
        {
            HUD = GetNodeOrNull<CanvasLayer>("HUD");
            if (HUD != null)
            {
                gameOverNode = HUD.GetNodeOrNull<Control>("GameOverNode");
                Dim = HUD.GetNodeOrNull<ColorRect>("Dim");
            }
            if (gameOverNode != null)
            {
                gameOverLabel = gameOverNode.GetNodeOrNull<Label>("GameOverLabel");
            }
            if (HUD == null)
            {
                GD.PrintErr("HUD not found in GameScene");
                return;
            }
            if (gameOverNode == null)
            {
                GD.PrintErr("GameOverNode not found in HUD");
                return;
            }
            if (gameOverLabel == null)
            {
                GD.PrintErr("gameOverLabel not found in GameOverNode");
                return;
            }
            if (Dim == null)
            {
                GD.PrintErr("Dim not found in HUD");
                return;
            }
            player = this.GetPlayer();
            if (player == null)
            {
                GD.PrintErr("Player not found in GameScene");
                return;
            }
            player.PlayerDead += PlayerDeath;

        }

        public override void _EnterTree()
        {
            Reset();
        }

        public override void _ExitTree()
        {
            if (player != null)
            {
                player.PlayerDead -= PlayerDeath;
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            if (Stats.data.EnemiesKilled >= _absorbXpInterval * _currentInterval)
            {
                _currentInterval++;
                Array<Node> GlobalXpOrbs = GetTree().GetNodesInGroup("XpOrbs");
                foreach (XpOrb orbs in GlobalXpOrbs)
                {
                    orbs.SawPlayer = true;
                }
                GD.Print("Mass absorb xp");
            }
        }


        private void PlayerDeath()
        {
            CallDeferred(MethodName.GameOver, $"Game Over");
        }

        private void GameOver(string message)
        {

            if (_isChangingScene || !IsInstanceValid(this)) return;
            _isChangingScene = true;
            if (gameOverLabel != null)
            {
                Dim.Visible = true;
                gameOverLabel.Text = message;
            }
            if (gameOverNode != null)
            {
                gameOverNode.Visible = true;
            }
            GD.Print("GameOver");
            GetTree().CreateTimer(.1).Timeout += PauseGame;
            GameOverEvent?.Invoke();
        }

        private void PauseGame()
        {
            GetTree().Paused = true;
        }

        private void Reset()
        {
            Stats.ResetValues();
            Enemy.Alive = 0;
            GetTree().Paused = false;
        }


    }
}
