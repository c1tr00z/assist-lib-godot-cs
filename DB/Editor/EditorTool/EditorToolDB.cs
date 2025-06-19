using System;
using Godot;
using projectwitch.addons.AssistLib.EditorTools.Scripts;

namespace AssistLib.DB.Editor.EditorTool;

[Tool]
[EditorTool("DB Tool")]
public class EditorToolDB : AssistLibEditorTool<EditorToolDBSaveData>, IEditorToolRuntimeUI {

    #region Accessors

    public bool forceReloadProject { get; private set; }

    #endregion

    #region AssistLibEditorTool Implementation

    public Type panelType => typeof(AssistLibEditorToolDBPanel);

    protected override EditorToolDBSaveData GetMySaveData() {
        return new EditorToolDBSaveData {
            forceReloadProject = forceReloadProject,
        };
    }

    protected override void LoadFromSaveData(EditorToolDBSaveData saveData) {
        forceReloadProject = saveData.forceReloadProject;
    }

    #endregion

    #region Class Implementation

    public void UpdateSettings(bool forceReloadProject) {
        this.forceReloadProject = forceReloadProject;
    }

    #endregion
}