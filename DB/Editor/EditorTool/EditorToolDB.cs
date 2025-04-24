using System.Collections.Generic;
using Godot;
using projectwitch.addons.AssistLib.EditorTools.Scripts;

namespace AssistLib.DB.Editor.EditorTool;

[Tool]
public class EditorToolDB : AssistLibEditorTool<EditorToolDBSaveData>, IEditorToolPredefinedScene {

    #region AssistLibEditorTool Implementation

    public string panelPath => "res://addons/AssistLib/DB/Editor/EditorTool/Scenes/assist_lib_db_panel.tscn";

    protected override EditorToolDBSaveData GetMySaveData() {
        return new EditorToolDBSaveData();
    }

    protected override void LoadFromSaveData(EditorToolDBSaveData saveData) {
    }

    #endregion
}