namespace SurvivorsGame
{
    using Godot;
    using Godot.Collections;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public partial class SceneLoader : CanvasLayer
    {
        public static SceneLoader Instance { get; private set; }
        private AnimationPlayer _animationPlayer;
        private AnimationPlayer _loadAnimation;
        private ProgressBar _progressBar;
        private Array<float> _progress = new();
        private bool _useSubThreads = false;
        private string _targetScenePath;
        private bool _isLoading = false;
        private bool _isPrewarming = false;
        private bool _prewarmPools;
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
            _animationPlayer.AnimationFinished += OnAnimationFinished;
            SetProcess(false);
        }

        public override void _ExitTree()
        {
            if (IsInstanceValid(_animationPlayer))
            {
                _animationPlayer.AnimationFinished -= OnAnimationFinished;
            }

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

            Error err = ResourceLoader.LoadThreadedRequest(
                _targetScenePath,
                "",
                _useSubThreads,
                ResourceLoader.CacheMode.IgnoreDeep
            );

            if (err == Error.Ok)
            {
                _isLoading = true;
                _isPrewarming = false;
                if (_progressBar != null) _progressBar.Value = 0;
                _animationPlayer.Play("fade");
                SetProcess(true);
            }
            else
            {
                GD.PrintErr($"SceneLoader: Failed to start loading '{_targetScenePath}'. Error: {err}");
            }
        }

        public override void _Process(double delta)
        {
            ResourceLoader.ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(_targetScenePath, (Godot.Collections.Array)_progress);

            switch (status)
            {
                case ResourceLoader.ThreadLoadStatus.InProgress:
                    float rawPct = _progress.Count > 0 ? _progress[0] * 100f : 0f;
                    float displayPct = _prewarmPools ? rawPct * 0.5f : rawPct;
                    if (_loadAnimation != null && !_loadAnimation.IsPlaying())
                    {
                        _loadAnimation.Play("load");
                    }
                    if (_progressBar != null) _progressBar.Value = displayPct;
                    break;

                case ResourceLoader.ThreadLoadStatus.Loaded:
                    SetProcess(false);
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
                if (_progressBar != null) _progressBar.Value = 50f;

                Node tempRoot = _loadedScene.Instantiate();
                var requirements = new List<(PackedScene scene, int count)>();
                CollectPrewarmRequirements(tempRoot, requirements);
                tempRoot.QueueFree();

                await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

                double budgetMs = OS.HasFeature("web") ? 3.5 : 6.0;
                await PoolManager.Instance.PrewarmAsync(requirements, ratio =>
                {
                    if (IsInstanceValid(_progressBar))
                    {
                        _progressBar.Value = 50f + (ratio * 50f);
                    }
                }, budgetMs);

                _isPrewarming = false;
            }
            else if (!_prewarmPools)
            {
                PoolManager.Instance?.ClearAllPools();
            }

            if (_progressBar != null) _progressBar.Value = 100f;
            _loadAnimation?.PlayBackwards("load");

            if (!_animationPlayer.IsPlaying() || (_loadAnimation != null && !_loadAnimation.IsPlaying()))
            {
                ChangeToLoadedScene();
            }
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

        private void OnAnimationFinished(StringName animationName)
        {
            if (animationName == "fade" && _isLoading && !_isPrewarming && _loadedScene != null)
            {
                ChangeToLoadedScene();
            }
        }

        private void ChangeToLoadedScene()
        {
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
