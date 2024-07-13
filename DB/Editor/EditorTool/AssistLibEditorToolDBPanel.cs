using Godot;

namespace AssistLib.DB.Editor.EditorTool;

[Tool]
[GlobalClass]
public partial class AssistLibEditorToolDBPanel : Control {

    #region Class Implementation

    public void CollectDBEntries() {
        DBEditorActions.CollectDBEntries();
    }

    #endregion
}