using System;
using System.Collections.Generic;
using System.Linq;
using c1tr00z.AssistLib.Common;
using Godot;

namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

[GlobalClass]
[Tool]
public partial class AssistLibToolsPanel : VBoxContainer {

    #region Private Fields

    private Control _toolsListNode;

    private float _defaultPanelHeight = 900;

    private float _panelHeightDelta = 0;

    private List<Node> _toolsPanels = new();

    #endregion
    
    #region Export Fields
    
    [Export] private NodePath _toolsListNodePath;

    #endregion

    #region Node Implementation

    public override void _EnterTree() {
        base._EnterTree();
        EditorToolsController.ToolRemoved += OnToolRemoved;
    }

    public override void _ExitTree() {
        EditorToolsController.ToolRemoved -= OnToolRemoved;
        base._ExitTree();
    }

    #endregion

    #region Class Implementation

    public void InitToolsPanels() {
        EditorToolsController.instance.tools.ForEach(t => {
            Node panelNode = null;
            if (t is IEditorToolPredefinedScene withPredefined) {
                panelNode = MakePanel(withPredefined);
            } else if (t is IEditorToolRuntimeUI withRuntime) {
                panelNode = MakePanel(withRuntime);
            } else {
                throw new Exception($"Tool panel class has to implement IEditorToolPredefinedScene or " +
                                    $"IEditorToolRuntimeUI: panel type - {t.GetType().FullName}");
            }
            _toolsPanels.Add(panelNode);
            AddChild(panelNode);
            // var panel = GD.Load<PackedScene>(t.panelPath).Instantiate();
            // _toolsContainer.AddChild(panel);
        });
        
        // if (this.TryGetCached(ref _toolsListNode, _toolsListNodePath)) {
        //     _toolsListNode.SetSize(new Vector2(_toolsListNode.Size.X, _defaultPanelHeight));
        // }
    }

    private Node MakePanel(IEditorToolPredefinedScene toolWithPredefinedScene) {
        return GD.Load<PackedScene>(toolWithPredefinedScene.panelPath).Instantiate();
    }

    private Node MakePanel(IEditorToolRuntimeUI toolWithRuntimeUI) {
        var panelObject = Activator.CreateInstance(toolWithRuntimeUI.panelType);
        if (panelObject is not IEditorToolPanelRuntime toolPanelRuntime) {
            throw new Exception("Tool panel has to implement EditorToolPanelRuntime<T>");
        }
        toolPanelRuntime.BuildPanel();
        return toolPanelRuntime as Node;
    }

    public void SaveTools() {
        EditorToolsController.instance.SaveTools();
    }

    public void IncreaseSize() {
        if (this.TryGetCached(ref _toolsListNode, _toolsListNodePath)) {
            _panelHeightDelta = Mathf.Min(3000, _panelHeightDelta + 10);
            _toolsListNode.SetSize(new Vector2(_toolsListNode.Size.X, _defaultPanelHeight + _panelHeightDelta));
        }
    }

    public void DecreaseSize() {
        if (this.TryGetCached(ref _toolsListNode, _toolsListNodePath)) {
            _panelHeightDelta = Mathf.Max(-600, _panelHeightDelta - 10);
            _toolsListNode.SetSize(new Vector2(_toolsListNode.Size.X, _defaultPanelHeight + _panelHeightDelta));
        }
    }

    private void OnToolRemoved(AssistLibEditorTool tool) {
        var toolsPanel = _toolsPanels.OfType<IAssistLibToolPanel>().FirstOrDefault(p => p.tool == tool);
        if (toolsPanel == null) {
            GD.PushWarning($"No panel for tool {tool.GetType().FullName}");
            return;
        }

        var panelNode = toolsPanel as Node;
        _toolsPanels.Remove(panelNode);
        RemoveChild(panelNode);
    }

    #endregion
}