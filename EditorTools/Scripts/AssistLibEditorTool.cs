using Godot;

namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

[Tool]
public abstract class AssistLibEditorTool {

    #region Accessors

    public abstract string panelPath { get; }

    #endregion

    #region Class Implementation

    public virtual void InitTool() {
        
    }

    #endregion
}