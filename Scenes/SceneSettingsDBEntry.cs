using AssistLib.DB.Runtime;
using c1tr00z.AssistLib.UI;
using Godot;

namespace c1tr00z.AssistLib.Scenes;

[Tool]
[GlobalClass]
public partial class SceneSettingsDBEntry : DBEntry {
    
    #region Export Fields

    [Export] public bool autoCloseLoadingScreen;

    [Export] public UIFrameDBEntry loadingScreenFrame;
    
    
    [Export] public double minLoadingTimeSeconds = 1;
    
    [Export] public bool unloadCurrentSceneBeforeLoading;

    #endregion
}