using System;
using System.Collections.Generic;
using Godot;

namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

[Tool]
public abstract class AssistLibEditorTool {

    #region Events

    public static event Action<AssistLibEditorTool> ToolLoaded;
    
    #endregion

    #region Accessors

    public abstract string panelPath { get; }

    #endregion

    #region Class Implementation

    public void SaveTool(Dictionary<string, object> json) {
        if (json == null) {
            return;
        }
        json.Add("type", GetType().FullName);
        SaveToolToJson(json);
    }

    protected abstract void SaveToolToJson(Dictionary<string, object> json);

    public void LoadTool(Dictionary<string, object> json) {
        LoadToolFromJson(json);
        ToolLoaded?.Invoke(this);
    }

    protected abstract void LoadToolFromJson(Dictionary<string, object> json);
    
    public void OnEnableTool() {}
    
    public void OnDisableTool() {}

    #endregion
}