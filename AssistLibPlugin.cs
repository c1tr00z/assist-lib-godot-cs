#if TOOLS
using Godot;
using System;

[Tool]
public partial class AssistLibPlugin : EditorPlugin {
    
    #region Private Fields

    private CanvasItem _mainPanelInstance = null;

    //TODO: fix it later
    private String _pathToWindow = "res://addons/AssistLib/EditorUI/Scenes/AssistLibUIRootPanel.tscn";

    #endregion

    public override void _EnterTree() {
        if (_mainPanelInstance == null) {
            var packedScene = GD.Load<PackedScene>(_pathToWindow);
            _mainPanelInstance = packedScene.Instantiate<CanvasItem>();
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