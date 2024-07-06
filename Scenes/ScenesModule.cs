using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssistLib.DB.Runtime;
using c1tr00z.AssistLib.Modules;
using c1tr00z.AssistLib.UI;
using Godot;

namespace projectwitch.addons.AssistLib.Scenes;

[GlobalClass]
public partial class ScenesModule : Module {

    #region Events

    public static event Action<SceneDBEntry> LoadingStarted;
    
    public static event Action<SceneDBEntry, double> LoadingProgress;

    public static event Action<SceneDBEntry, Node> LoadingFinished;
    
    #endregion

    #region Private Fields

    private Queue<SceneDBEntry> _requests = new();

    private DBLoadResult<PackedScene> _currentRequest = null;

    private bool _isLoading = false;

    #endregion

    #region Accessors
    
    private SceneSettingsDBEntry settings { get; set; }

    public Node currentScene { get; private set; }

    #endregion

    #region Node Implementation

    public override void _Ready() {
        settings = DB.Get<SceneSettingsDBEntry>();
        base._Ready();
    }

    #endregion

    #region Class Implementation

    public void LoadScene(SceneDBEntry dbEntry) {
        if (_currentRequest != null && _currentRequest.result == LoadResult.Loading) {
            return;
        }
        _requests.Enqueue(dbEntry);

        ProcessLoad();
    }

    public async Task ProcessLoad() {
        if (_isLoading) {
            return;
        }

        _isLoading = true;

        while (_requests.Count > 0) {
            var dbEntry = _requests.Dequeue();

            settings.loadingScreenFrame?.Show();

            LoadingStarted?.Invoke(dbEntry);

            var startTime = Time.Singleton.GetUnixTimeFromSystem();
            var currentTime = startTime;
            
            GetTree().UnloadCurrentScene();

            var request = dbEntry.LoadSceneRequestAsync();

            while (request.result == LoadResult.Loading) {
                LoadingProgress?.Invoke(dbEntry, request.progress);
                await Task.Delay(10);
            }

            GetTree().ChangeSceneToPacked(request.resource);

            currentScene = GetTree().CurrentScene;
            
            currentTime = Time.Singleton.GetUnixTimeFromSystem();

            var timeDiff = currentTime - startTime;
            
            if (timeDiff < settings.minLoadingTimeSeconds) {
                await Task.Delay(Mathf.CeilToInt((settings.minLoadingTimeSeconds - timeDiff) * 1000));
            }
            
            LoadingProgress?.Invoke(dbEntry, 1);
            
            LoadingFinished?.Invoke(dbEntry, currentScene);

            if (settings.autoCloseLoadingScreen) {
                HideLoadingScreen();
            }
        }

        _isLoading = false;
    }

    public void HideLoadingScreen() {
        settings.loadingScreenFrame.TryHide();
    }

    #endregion
}