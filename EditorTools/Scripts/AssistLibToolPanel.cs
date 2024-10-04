using c1tr00z.AssistLib.Common;
using Godot;

namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

public abstract partial class AssistLibToolPanel<T> : Control where T : AssistLibEditorTool {

    #region Private Fields

    private T _tool;

    #endregion
    
    #region Accessors

    protected T tool => CommonExt.GetCached(ref _tool, EditorToolsController.Get<T>);

    protected bool isPanelActive => this.FindInParentsByType<AssistLibToolsPanel>() != null;

    #endregion

    #region Node Implementation

    public override void _EnterTree() {
        base._EnterTree();
        EditorToolsController.RequestData += OnRequestToolData;
        AssistLibEditorTool.ToolLoaded += OnPanelToolLoaded;
        OnPanelToolLoaded(tool);
    }

    public override void _ExitTree() {
        EditorToolsController.RequestData -= OnRequestToolData;
        AssistLibEditorTool.ToolLoaded -= OnPanelToolLoaded;
        base._ExitTree();
    }

    #endregion

    #region Class Implementation

    private void OnRequestToolData() {
        if (!isPanelActive) {
            return;
        }
        OnRequestData();
    }
    
    protected abstract void OnRequestData();

    protected void OnPanelToolLoaded(AssistLibEditorTool tool) {
        if (!isPanelActive) {
            return;
        }

        OnToolLoaded(tool);
    }
    
    protected abstract void OnToolLoaded(AssistLibEditorTool tool);

    #endregion
}