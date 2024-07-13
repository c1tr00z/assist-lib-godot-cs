using c1tr00z.AssistLib.Common;
using Godot;

namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

[GlobalClass]
public partial class AssistLibToolsPanel : Control { //assist_lib_tools_panel_scene

    #region Private Fields

    private Node _toolsContainer;

    #endregion
    
    #region Export Fields

    [Export] private NodePath _toolsContainerPath;

    #endregion
    
    #region Control Implementation
    
    public override void _EnterTree() {
        base._EnterTree();
        InitToolsPanels();
    }

    #endregion

    #region Class Implementation

    private void InitToolsPanels() {
        GD.Print("InitToolsPanels 1");
        if (this.TryGetCached(ref _toolsContainer, _toolsContainerPath)) {
            GD.Print("InitToolsPanels 2");
            EditorToolsController.instance.tools.ForEach(t => {
                var panel = GD.Load<PackedScene>(t.panelPath).Instantiate();
                GD.Print($"InitToolsPanels 3 -> {panel}");
                _toolsContainer.AddChild(panel);
            });
        }
    }

    #endregion
}