#if TOOLS

using AssistLib.DB.Editor;
using Godot;

namespace AssistLib.EditorUI;

[Tool]
[GlobalClass]
public partial class AssistLibUIRootPanel : CenterContainer {
    
    #region Class Implementation

    public void CollectDBEntries() {
        DBEditorActions.CollectDBEntries();
    }

    #endregion
}
#endif
