#if TOOLS
using Godot;
using System;
using projectwitch.addons.AssistLib.EditorTools.Scripts;

[Tool]
public partial class AssistLibPlugin : EditorPlugin {
    
    #region Private Fields

    private Control _mainPanelInstance = null;

    //TODO: showing tools panel for now
    private String _pathToWindow = "res://addons/AssistLib/EditorTools/Scenes/assist_lib_tools_panel_scene.tscn";

    #endregion

    public override void _EnterTree() {
        if (_mainPanelInstance == null) {
            var packedScene = GD.Load<PackedScene>(_pathToWindow);
            _mainPanelInstance = packedScene.Instantiate<Control>();
            if (_mainPanelInstance is AssistLibToolsPanel toolsPanel) {
                toolsPanel.InitToolsPanels();
            } else {
                GD.Print(_mainPanelInstance.GetType());
            }
        }

        EditorInterface.Singleton.GetEditorMainScreen().AddChild(_mainPanelInstance);
        _MakeVisible(false);
    }

    public override void _ExitTree() {
        if (_mainPanelInstance != null) {
            _mainPanelInstance.QueueFree();
        }
    }

    public override bool _HasMainScreen() {
        return true;
    }

    public override void _MakeVisible(bool visible) {
        if (_mainPanelInstance == null) {
            return;
        }

        _mainPanelInstance.Visible = visible;
    }

    public override string _GetPluginName() {
        return "AssistLib";
    }

    public override Texture2D _GetPluginIcon() {
        return EditorInterface.Singleton.GetBaseControl().GetThemeIcon("Node", "EditorIcons");
    }
}
#endif