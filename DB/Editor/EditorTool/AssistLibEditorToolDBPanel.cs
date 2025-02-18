using Godot;

namespace AssistLib.DB.Editor.EditorTool;

[Tool]
[GlobalClass]
public partial class AssistLibEditorToolDBPanel : VBoxContainer {

    #region Class Implementation

    public void CollectDBEntries() {
#if TOOLS
        DBEditorActions.CollectDBEntries();
#endif
    }
    
    public void ForceReload() {
#if TOOLS
        Runtime.DB.ForceReload();
#endif
    }

    #endregion
}