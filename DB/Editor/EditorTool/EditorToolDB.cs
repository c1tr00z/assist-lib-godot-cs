using System;
using Godot;
using projectwitch.addons.AssistLib.EditorTools.Scripts;

namespace AssistLib.DB.Editor.EditorTool;

[Tool]
[EditorTool("DB Tool")]
public class EditorToolDB : AssistLibEditorTool<EditorToolDBSaveData>, IEditorToolRuntimeUI {

    #region AssistLibEditorTool Implementation

    public Type panelType => typeof(AssistLibEditorToolDBPanel);

    protected override EditorToolDBSaveData GetMySaveData() {
        return new EditorToolDBSaveData();
    }

    protected override void LoadFromSaveData(EditorToolDBSaveData saveData) {
    }

    #endregion
}