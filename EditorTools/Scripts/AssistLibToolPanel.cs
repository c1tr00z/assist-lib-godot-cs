using c1tr00z.AssistLib.Common;
using Godot;

namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

public abstract partial class AssistLibToolPanel<T> : Control where T : AssistLibEditorTool {

    #region Private Fields

    private T _tool;

    #endregion
    
    #region Accessors

    protected T tool => CommonExt.GetCached(ref _tool, EditorToolsController.Get<T>);

    #endregion

    #region Node Implementation

    public override void _EnterTree() {
        base._EnterTree();
        AssistLibEditorTool.ToolLoaded += OnToolLoaded;
        OnToolLoaded(tool);
    }

    public override void _ExitTree() {
        base._ExitTree();
        AssistLibEditorTool.ToolLoaded -= OnToolLoaded;
    }

    #endregion

    #region Class Implementation

    protected abstract void OnToolLoaded(AssistLibEditorTool tool);

    #endregion
}