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
        EditorToolsController.RequestData += OnRequestData;
        AssistLibEditorTool.ToolLoaded += OnToolLoaded;
        OnToolLoaded(tool);
    }

    public override void _ExitTree() {
        base._ExitTree();
        EditorToolsController.RequestData -= OnRequestData;
        AssistLibEditorTool.ToolLoaded -= OnToolLoaded;
    }

    #endregion

    #region Class Implementation

    protected abstract void OnRequestData();

    protected abstract void OnToolLoaded(AssistLibEditorTool tool);

    #endregion
}