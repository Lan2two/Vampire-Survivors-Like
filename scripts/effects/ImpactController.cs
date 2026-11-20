namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;

    public partial class ImpactController : ColorRect
    {
        [Export] public float SplitOffset { get; set; } = 0.0f;
        [Export] public float ImpactFlash { get; set; } = 0.0f;
        [Export] public bool InvertImpact { get; set; } = false;
        [Export] public float SpeedLineIntensity { get; set; } = 0.0f;
        [Export] public float BwCutoff { get; set; } = 0.5f;
        [Export] public Vector2 ShakeOffset { get; set; } = Vector2.Zero;

        private ShaderMaterial _shaderMaterial;
        private AnimationPlayer _animationPlayer;

        public override void _Ready()
        {
            ProcessMode = ProcessModeEnum.Always;
            _animationPlayer = GetNodeOrNull<AnimationPlayer>("../AnimationPlayer");

            if (Material is ShaderMaterial originalMaterial)
            {
                _shaderMaterial = (ShaderMaterial)originalMaterial.Duplicate();
                Material = _shaderMaterial;
            }

            ApplyShaderParameters();

            //GetTree().CreateTimer(1, false).Timeout += () => { Impact(new Vector2(0.5f, -0.5f)); };
            EventBus.WorldSlashEvent += OnWorldSlash;
        }

        public override void _Process(double delta)
        {
            ApplyShaderParameters();
        }

        private void ApplyShaderParameters()
        {
            if (_shaderMaterial == null)
            {
                return;
            }

            _shaderMaterial.SetShaderParameter("split_offset", SplitOffset);
            _shaderMaterial.SetShaderParameter("impact_flash", ImpactFlash);
            _shaderMaterial.SetShaderParameter("invert_impact", InvertImpact);
            _shaderMaterial.SetShaderParameter("speed_line_intensity", SpeedLineIntensity);
            _shaderMaterial.SetShaderParameter("bw_cutoff", BwCutoff);
            _shaderMaterial.SetShaderParameter("shake_offset", ShakeOffset);
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
            Array<Node> globalEnemies = GetTree().GetNodesInGroup("enemies");
            foreach (Enemy enemy in globalEnemies)
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
