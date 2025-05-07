using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

    private OptionButton _toolsListButton = null;

    private Button _saveButton;

    private VBoxContainer _toolsContainer;

    private VBoxContainer _toolsTypesContainer;

    #endregion
    
    #region Export Fields
    
    [Export] private NodePath _toolsListNodePath;

    [Export] private NodePath _toolsListButtonPath;

    #endregion

    #region Node Implementation

    public override void _EnterTree() {
        base._EnterTree();
        EditorToolsController.ToolAdded += AddPanelFor;
        EditorToolsController.ToolRemoved += OnToolRemoved;
    }

    public override void _ExitTree() {
        EditorToolsController.ToolAdded -= AddPanelFor;
        EditorToolsController.ToolRemoved -= OnToolRemoved;
        base._ExitTree();
    }

    #endregion

    #region Class Implementation

    public void InitToolsPanels() {
        BuildPanel();
        EditorToolsController.instance.tools.ForEach(AddPanelFor);
    }

    private void BuildPanel() {
        var headerLabel = EditorToolsUI.MakeLabel("Tools", true);
        _saveButton = EditorToolsUI.MakeButton("Save", SaveTools, false);
        var controlsContainer = EditorToolsUI.MakeHBoxContainer(headerLabel, _saveButton);
        AddChild(controlsContainer);
        
        var allToolsTypes = EditorToolsController.instance.allToolsTypes;
        _toolsListButton = EditorToolsUI.MakeOptionsButton(allToolsTypes.Keys, text => text, true);
        var addToolButton = EditorToolsUI.MakeButton("Add tool", AddSelectedTool);
        
        AddChild(EditorToolsUI.MakeHBoxContainer(EditorToolsUI.MakeLabel("Add tool"), _toolsListButton, addToolButton));
        
        AddChild(new HSeparator());
        AddChild(new HSeparator());
        _toolsContainer = new VBoxContainer();
        var toolsScrollContainer = EditorToolsUI.MakeScrollContainer(true, true, _toolsContainer);
        AddChild(toolsScrollContainer);
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

    public void AddSelectedTool() {
        if (this.TryGetCached(ref _toolsListButton, _toolsListButtonPath)) {
            var toolName = _toolsListButton.GetItemText(_toolsListButton.Selected);
            if (EditorToolsController.instance.allToolsTypes.TryGetValue(toolName, out Type toolType)) {
                EditorToolsController.instance.AddTool(toolType);
            }
        }
    }

    private void AddPanelFor(AssistLibEditorTool tool) {
        Node panelNode = null;
        if (tool is IEditorToolPredefinedScene withPredefined) {
            panelNode = MakePanel(withPredefined);
        } else if (tool is IEditorToolRuntimeUI withRuntime) {
            panelNode = MakePanel(withRuntime);
        } else {
            throw new Exception($"Tool panel class has to implement IEditorToolPredefinedScene or " +
                                $"IEditorToolRuntimeUI: panel type - {tool.GetType().FullName}");
        }
        _toolsPanels.Add(panelNode);
        AddChild(panelNode);
    }

    #endregion
}