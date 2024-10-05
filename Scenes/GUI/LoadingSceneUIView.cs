using c1tr00z.AssistLib.Modules;
using c1tr00z.AssistLib.UI;
using Godot;

namespace c1tr00z.AssistLib.Scenes.GUI;

[GlobalClass]
public partial class LoadingSceneUIView : UIViewItem<SceneDBEntry> {

    #region Class Implementation

    public void LoadScene() {
        Modules.Modules.Get<ScenesModule>().LoadScene(item);
    }

    #endregion
}