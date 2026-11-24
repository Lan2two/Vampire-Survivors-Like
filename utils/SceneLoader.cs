namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public partial class SceneLoader : CanvasLayer
    {
        public static SceneLoader Instance { get; private set; }

        private AnimationPlayer _animationPlayer;
        private AnimationPlayer _loadAnimation;
        private ProgressBar _progressBar;

        private Array _progress = new();
        private bool _useSubThreads = false;
        private string _targetScenePath;
        private bool _isLoading = false;
        private bool _isPrewarming = false;
        private bool _prewarmPools;
        private double _targetProgress = 0.0;
        private PackedScene _loadedScene;

        public override void _Ready()
        {
            _animationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
            _loadAnimation = GetNodeOrNull<AnimationPlayer>("LoadAnimation");
            _progressBar = GetNodeOrNull<ProgressBar>("ProgressBar");

            if (_animationPlayer == null)
            {
                GD.PrintErr("SceneLoader: AnimationPlayer not found.");
                return;
            }

            Instance = this;
            SetProcess(false);
        }

        public override void _ExitTree()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void LoadScene(string targetScene, bool prewarmPools = false)
        {
            if (_isLoading) return;

            if (string.IsNullOrWhiteSpace(targetScene))
            {
                GD.PrintErr("SceneLoader: Scene path is empty.");
                return;
            }

            if (!IsInstanceValid(_animationPlayer))
            {
                GD.PrintErr("SceneLoader: Cannot load a scene without an AnimationPlayer.");
                return;
            }

            _targetScenePath = targetScene;
            _prewarmPools = prewarmPools;
            _targetProgress = 0.0;

            Error err = ResourceLoader.LoadThreadedRequest(
                _targetScenePath,
                "",
                _useSubThreads,
                ResourceLoader.CacheMode.Reuse
            );

            if (err == Error.Ok)
            {
                _isLoading = true;
                _isPrewarming = false;

                if (_progressBar != null)
                {
                    _progressBar.Visible = true;
                    _progressBar.Value = 0;
                }

                _animationPlayer.Play("fade");
                _loadAnimation?.Play("load");
                SetProcess(true);
            }
            else
            {
                GD.PrintErr($"SceneLoader: Failed to start loading '{_targetScenePath}'. Error: {err}");
            }
        }

        public override void _Process(double delta)
        {
            if (_progressBar != null)
            {
                _progressBar.Value = Mathf.Lerp(_progressBar.Value, _targetProgress, delta * 10.0);
            }

            if (_loadedScene != null) return;

            ResourceLoader.ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(_targetScenePath, _progress);

            switch (status)
            {
                case ResourceLoader.ThreadLoadStatus.InProgress:
                    float rawPct = _progress.Count > 0 ? (float)_progress[0] * 100f : 0f;
                    _targetProgress = _prewarmPools ? rawPct * 0.5f : rawPct;
                    break;

                case ResourceLoader.ThreadLoadStatus.Loaded:
                    _loadedScene = ResourceLoader.LoadThreadedGet(_targetScenePath) as PackedScene;
                    if (_loadedScene == null)
                    {
                        GD.PrintErr($"SceneLoader: Loaded resource is not a PackedScene: {_targetScenePath}");
                        CancelLoad();
                        break;
                    }
                    _ = FinishLoadingAsync();
                    break;

                case ResourceLoader.ThreadLoadStatus.Failed:
                case ResourceLoader.ThreadLoadStatus.InvalidResource:
                    GD.PrintErr($"SceneLoader: Failed to load scene '{_targetScenePath}'. Status: {status}");
                    CancelLoad();
                    if (_progressBar != null) _progressBar.Visible = false;
                    break;
            }
        }

        private async Task FinishLoadingAsync()
        {
            if (_prewarmPools && PoolManager.Instance != null)
            {
                _isPrewarming = true;
                _targetProgress = 50.0;

                Node tempRoot = _loadedScene.Instantiate();
                var requirements = new List<(PackedScene scene, int count)>();
                CollectPrewarmRequirements(tempRoot, requirements);
                tempRoot.QueueFree();

                await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

                double budgetMs = OS.HasFeature("web") ? 3.5 : 6.0;
                await PoolManager.Instance.PrewarmAsync(requirements, ratio =>
                {
                    _targetProgress = 50.0 + (ratio * 50.0);
                }, budgetMs);

                _isPrewarming = false;
            }
            else if (!_prewarmPools)
            {
                PoolManager.Instance?.ClearAllPools();
            }

            _targetProgress = 100.0;

            if (_animationPlayer.IsPlaying())
            {
                await ToSignal(_animationPlayer, AnimationMixer.SignalName.AnimationFinished);
            }

            if (_progressBar != null)
            {
                double elapsed = 0.0;
                while (_progressBar.Value < 99.5 && elapsed < 0.5)
                {
                    await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
                    elapsed += GetProcessDeltaTime();
                }
                _progressBar.Value = 100.0;
            }

            if (_loadAnimation != null && _loadAnimation.HasAnimation("load"))
            {
                _loadAnimation.PlayBackwards("load");
                await ToSignal(_loadAnimation, AnimationMixer.SignalName.AnimationFinished);
            }

            SetProcess(false);
            ChangeToLoadedScene();
        }

        private static void CollectPrewarmRequirements(Node node, List<(PackedScene, int)> results)
        {
            if (node is IPoolPrewarmer prewarmer)
            {
                results.AddRange(prewarmer.GetPoolRequirements());
            }

            foreach (Node child in node.GetChildren())
            {
                CollectPrewarmRequirements(child, results);
            }
        }

        private void ChangeToLoadedScene()
        {
            if (_loadedScene == null) return;

            SceneTree tree = GetTree();
            tree.SceneChanged += OnSceneChanged;
            tree.Paused = false;

            Error error = tree.ChangeSceneToPacked(_loadedScene);
            if (error != Error.Ok)
            {
                tree.SceneChanged -= OnSceneChanged;
                GD.PrintErr($"SceneLoader: Failed to change to scene '{_targetScenePath}'. Error: {error}");
                CancelLoad();
            }
        }

        private void OnSceneChanged()
        {
            GetTree().SceneChanged -= OnSceneChanged;
            SetProcess(false);
            _loadedScene = null;
            _isLoading = false;
            _isPrewarming = false;
            _animationPlayer.PlayBackwards("fade");
        }

        private void CancelLoad()
        {
            SetProcess(false);
            _loadedScene = null;
            _isLoading = false;
            _isPrewarming = false;
            _animationPlayer.PlayBackwards("fade");
        }
    }
}
