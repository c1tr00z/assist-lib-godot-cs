using AssistLib.DB.Runtime;
using Godot;
using projectwitch.addons.AssistLib.EditorTools.Scripts;

namespace AssistLib.DB.Editor.EditorTool;

[Tool]
[GlobalClass]
public partial class AssistLibEditorToolDBPanel : EditorToolPanelRuntime<EditorToolDB> {

    private CheckBox _toggleForceReloadProject = null;

    #region EditorToolPanelRuntime Implementation

    protected override void OnRequestData() { }

    protected override void OnToolLoaded(AssistLibEditorTool tool) { }

    protected override void BuildPanelWidgets() {
        _toggleForceReloadProject = EditorToolsUI.MakeCheckBox("Force reload project on collect", editorTool.forceReloadProject, OnToggleReloadProject);
        AddChild(_toggleForceReloadProject);
        AddChild(EditorToolsUI.MakeButton("Collect DB Entries", CollectDBEntries));
        AddChild(EditorToolsUI.MakeButton("Force reload", ForceReload));
    }

    #endregion

    #region Class Implementation

    public void CollectDBEntries() {
#if TOOLS
        DBEditorActions.CollectDBEntries();
        if (editorTool.forceReloadProject) {
            EditorInterface.Singleton.RestartEditor(true);
        }
#endif
    }

    public void ForceReload() {
#if TOOLS
        EditorInterface.Singleton.RestartEditor(true);
#endif
    }

    private void OnToggleReloadProject(bool isReloadProject) {
        editorTool.UpdateSettings(isReloadProject);
    }

    #endregion
}