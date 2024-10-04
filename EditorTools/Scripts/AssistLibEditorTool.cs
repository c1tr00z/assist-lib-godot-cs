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

    public abstract IEditorToolData GetSaveData();

    public bool LoadTool(IEditorToolData saveData) {
        if (!LoadToolFromSaveData(saveData)) {
            return false;
        }
        ToolLoaded?.Invoke(this);
        return true;
    }

    protected abstract bool LoadToolFromSaveData(IEditorToolData saveData);
    
    public void OnEnableTool() {}
    
    public void OnDisableTool() {}

    #endregion
}

public abstract class AssistLibEditorTool<T> : AssistLibEditorTool where T : IEditorToolData {

    #region AssistLibEditorTool Implementation

    public override IEditorToolData GetSaveData() {
        return GetMySaveData();
    }

    protected override bool LoadToolFromSaveData(IEditorToolData saveData) {
        if (saveData == null || !(saveData is T mySaveData)) {
            return false;
        }

        LoadFromSaveData(mySaveData);
        return true;
    }

    #endregion
    
    #region Accessors

    protected abstract T GetMySaveData();

    protected abstract void LoadFromSaveData(T saveData);

    #endregion
}