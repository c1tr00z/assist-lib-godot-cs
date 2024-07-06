using c1tr00z.AssistLib.Common;
using c1tr00z.AssistLib.Modules;
using c1tr00z.AssistLib.UI;
using Godot;
using Godot.Collections;

namespace projectwitch.addons.AssistLib.Scenes.GUI;

[GlobalClass]
public partial class LoadingScreenUIView : UIViewBase {

    #region Private Fields

    private ProgressBar _progressBar;

    #endregion
    
    #region Export Fields

    [Export] private NodePath _progressBarPath;

    [Export] private Array<NodePath> _showOnLoadControls = new();

    #endregion
    
    #region UIViewBase Implementation

    protected override void OnArgs(params object[] args) {
        foreach (var controlPath in _showOnLoadControls) {
            GetNode(controlPath).DisableNode();
        }
    }

    protected override void OnHide() {
        foreach (var controlPath in _showOnLoadControls) {
            GetNode(controlPath).DisableNode();
        }
        base.OnHide();
    }

    protected override void SubscribeToEvents() {
        base.SubscribeToEvents();
        ScenesModule.LoadingStarted += ScenesModuleOnLoadingStarted;
        ScenesModule.LoadingProgress += ScenesModuleOnLoadingProgress;
        ScenesModule.LoadingFinished += ScenesModuleOnLoadingFinished;
    }

    protected override void UnsubscribeFromEvents() {
        base.UnsubscribeFromEvents();
        ScenesModule.LoadingStarted -= ScenesModuleOnLoadingStarted;
        ScenesModule.LoadingProgress -= ScenesModuleOnLoadingProgress;
        ScenesModule.LoadingFinished -= ScenesModuleOnLoadingFinished;
    }

    #endregion

    #region Class Implementation

    private void ScenesModuleOnLoadingStarted(SceneDBEntry obj) {
        if (this.TryGetCached(ref _progressBar, _progressBarPath)) {
            _progressBar.Value = 0;
        }
        foreach (var controlPath in _showOnLoadControls) {
            GetNode(controlPath).DisableNode();
        }
    }

    private void ScenesModuleOnLoadingProgress(SceneDBEntry sceneDbEntry, double progress) {
        if (this.TryGetCached(ref _progressBar, _progressBarPath)) {
            _progressBar.Value = progress;
        }
    }

    private void ScenesModuleOnLoadingFinished(SceneDBEntry sceneDbEntry, Node progress) {
        foreach (var controlPath in _showOnLoadControls) {
            GetNode(controlPath).EnableNode();
        }
    }

    public void HideLoadingScreen() {
        Modules.Get<ScenesModule>().HideLoadingScreen();
    }

    #endregion
}