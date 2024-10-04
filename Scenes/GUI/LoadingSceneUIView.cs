using c1tr00z.AssistLib.Modules;
using c1tr00z.AssistLib.UI;
using Godot;

namespace projectwitch.addons.AssistLib.Scenes.GUI;

[GlobalClass]
public partial class LoadingSceneUIView : UIViewItem<SceneDBEntry> {

    #region Class Implementation

    public void LoadScene() {
        Modules.Get<ScenesModule>().LoadScene(item);
    }

    #endregion
}