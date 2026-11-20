namespace SurvivorsGame
{
    using Godot;
    public partial class SceneLoader : CanvasLayer
    {
        public static SceneLoader Instance { get; private set; }
        private AnimationPlayer _animationPlayer;
        private Godot.Collections.Array _progress = new();
        private bool _useSubThreads = false;
        private string _targetScenePath;
        private bool _isLoading = false;
        private PackedScene _loadedScene;

        public override void _Ready()
        {
            _animationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
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

        public void LoadScene(string targetScene)
        {
            if (_isLoading)
            {
                return;
            }

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
            Error err = ResourceLoader.LoadThreadedRequest(
                _targetScenePath,
                "",
                _useSubThreads,
                ResourceLoader.CacheMode.IgnoreDeep
            );
            if (err == Error.Ok)
            {
                _isLoading = true;
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
            ResourceLoader.ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(_targetScenePath, _progress);

            switch (status)
            {
                case ResourceLoader.ThreadLoadStatus.InProgress:
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

                    if (!_animationPlayer.IsPlaying())
                    {
                        ChangeToLoadedScene();
                    }
                    break;

                case ResourceLoader.ThreadLoadStatus.Failed:
                case ResourceLoader.ThreadLoadStatus.InvalidResource:
                    GD.PrintErr($"SceneLoader: Failed to load scene '{_targetScenePath}'. Status: {status}");
                    CancelLoad();
                    break;
            }
        }

        private void OnAnimationFinished(StringName animationName)
        {
            if (animationName == "fade" && _isLoading && _loadedScene != null)
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
            _animationPlayer.PlayBackwards("fade");
        }

        private void CancelLoad()
        {
            SetProcess(false);
            _loadedScene = null;
            _isLoading = false;
            _animationPlayer.PlayBackwards("fade");
        }
    }
}