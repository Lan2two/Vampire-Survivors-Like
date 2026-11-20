namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    public partial class ImpactController : ColorRect
    {
        private ShaderMaterial _shaderMaterial;
        private AnimationPlayer _animationPlayer;

        public override void _Ready()
        {
            _animationPlayer = GetNodeOrNull<AnimationPlayer>("../AnimationPlayer");
            if (Material is ShaderMaterial originalMaterial)
            {
                _shaderMaterial = (ShaderMaterial)originalMaterial.Duplicate();
                Material = _shaderMaterial;
            }
            //GetTree().CreateTimer(1, false).Timeout += () => { Impact(new Vector2(0.5f, -0.5f)); };
            EventBus.WorldSlashEvent += OnWorldSlash;
        }

        public override void _ExitTree()
        {
            EventBus.WorldSlashEvent -= OnWorldSlash;
        }

        private void OnWorldSlash()
        {
            Impact(new Vector2(0.5f, -0.5f));
        }


        public void Impact(Vector2 sliceDirection)
        {
            if (_shaderMaterial == null || _animationPlayer == null)
            {
                return;
            }
            _shaderMaterial.SetShaderParameter("split_normal", sliceDirection);
            _animationPlayer.Stop();
            _animationPlayer.Play("impact");
        }

        public void KillEnemies()
        {
            Array<Node> GlobalEnemies = GetTree().GetNodesInGroup("enemies");
            foreach (Enemy enemy in GlobalEnemies)
            {
                Attack attack = new Attack
                {
                    Damage = 999
                };
                enemy.healthComponent.TakeDamage(attack);
            }
        }

        public void PauseGame(bool state)
        {
            GetTree().Paused = state;
        }
    }
}
