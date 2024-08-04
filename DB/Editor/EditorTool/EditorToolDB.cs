using System.Collections.Generic;
using Godot;
using projectwitch.addons.AssistLib.EditorTools.Scripts;

namespace AssistLib.DB.Editor.EditorTool;

[Tool]
public class EditorToolDB : AssistLibEditorTool {

    #region AssistLibEditorTool Implementation

    public override string panelPath => "res://addons/AssistLib/DB/Editor/EditorTool/Scenes/assist_lib_db_panel.tscn";

    protected override void LoadToolFromJson(Dictionary<string, object> json) {
    }

    protected override void SaveToolToJson(Dictionary<string, object> json) {
    }

    #endregion

}