using System.Linq;
using System.Reflection;
using c1tr00z.AssistLib.Common;
using Godot;

namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

public abstract partial class AssistLibToolPanel<T> : VBoxContainer, IAssistLibToolPanel where T : AssistLibEditorTool {

    #region Private Fields

    private string _toolTitle = null;

    #endregion
    
    #region Accessors

    protected T editorTool { get; private set; }

    protected bool isPanelActive => this.FindInParentsByType<AssistLibToolsPanel>() != null;

    protected string toolTitle => CommonExt.GetCached(ref _toolTitle, () => {
        var toolType = tool.GetType();
        var attribute = toolType.GetCustomAttributes<EditorToolAttribute>().FirstOrDefault();
        if (attribute is null) {
            return toolType.Name;
        }

        return attribute.toolTitle;
    });

    #endregion

    #region Node Implementation

    public override void _EnterTree() {
        base._EnterTree();
        EditorToolsController.RequestData += OnRequestToolData;
        AssistLibEditorTool.ToolLoaded += OnPanelToolLoaded;
        OnPanelToolLoaded(editorTool);
    }

    public override void _ExitTree() {
        EditorToolsController.RequestData -= OnRequestToolData;
        AssistLibEditorTool.ToolLoaded -= OnPanelToolLoaded;
        base._ExitTree();
    }

    #endregion

    #region IAssistLibToolPanel Implementation

    public AssistLibEditorTool tool => editorTool;

    public void Init(AssistLibEditorTool tool) {
        editorTool = tool as T;
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

    protected void RemoveTool() {
        EditorToolsController.instance.Remove(tool);
    }

    #endregion
}