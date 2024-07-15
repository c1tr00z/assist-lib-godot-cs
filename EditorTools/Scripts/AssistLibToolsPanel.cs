using c1tr00z.AssistLib.Common;
using Godot;

namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

[GlobalClass]
[Tool]
public partial class AssistLibToolsPanel : Control { //assist_lib_tools_panel_scene

    #region Private Fields

    private Node _toolsContainer;

    private Control _toolsListNode;

    private float _defaultPanelHeight = 900;

    private float _panelHeightDelta = 0;

    #endregion
    
    #region Export Fields

    [Export] private NodePath _toolsContainerPath;
    
    [Export] private NodePath _toolsListNodePath;

    #endregion

    #region Class Implementation

    public void InitToolsPanels() {
        if (this.TryGetCached(ref _toolsContainer, _toolsContainerPath)) {
            EditorToolsController.instance.tools.ForEach(t => {
                var panel = GD.Load<PackedScene>(t.panelPath).Instantiate();
                _toolsContainer.AddChild(panel);
            });
        }
        
        if (this.TryGetCached(ref _toolsListNode, _toolsListNodePath)) {
            _toolsListNode.SetSize(new Vector2(_toolsListNode.Size.X, _defaultPanelHeight));
        }
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

    #endregion
}