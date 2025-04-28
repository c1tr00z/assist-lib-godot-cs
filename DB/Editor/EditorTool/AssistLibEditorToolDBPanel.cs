using AssistLib.DB.Runtime;
using Godot;
using projectwitch.addons.AssistLib.EditorTools.Scripts;

namespace AssistLib.DB.Editor.EditorTool;

[Tool]
[GlobalClass]
public partial class AssistLibEditorToolDBPanel : EditorToolPanelRuntime<EditorToolDB> {
    
    #region EditorToolPanelRuntime Implementation

    protected override void OnRequestData() { }
    
    protected override void OnToolLoaded(AssistLibEditorTool tool) { }

    protected override void BuildPanelWidgets() {
        AddChild(EditorToolsUI.MakeButton("Collect DB Entries", CollectDBEntries));
        AddChild(EditorToolsUI.MakeButton("Force reload", ForceReload));
    }

    #endregion

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