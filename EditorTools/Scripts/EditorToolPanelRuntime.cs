using Godot;

namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

public abstract partial class EditorToolPanelRuntime<T> : AssistLibToolPanel<T>, IEditorToolPanelRuntime
    where T : AssistLibEditorTool {

    #region Private Fields

    private Button _closeButton;

    #endregion
    
    #region Class Implementation

    public void BuildPanel() {
        SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(BuildHeaderPanel());
        BuildPanelWidgets();
    }

    private Node BuildHeaderPanel() {
        var headerContainer = new PanelContainer();
        headerContainer.AddThemeStyleboxOverride(EditorToolsPanelsConstants.PANEL_HEADER_STYLE_NAME,
            EditorToolsPanelsConstants.PANEL_HEADER_STYLE);
        var horizontalContainer = new HBoxContainer();
        headerContainer.AddChild(horizontalContainer);
        horizontalContainer.LayoutMode = 2;
        horizontalContainer.AnchorsPreset = (int)LayoutPreset.FullRect;
        
        var titleLabel = new Label();
        titleLabel.Text = toolTitle;
        titleLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        horizontalContainer.AddChild(titleLabel);

        _closeButton = new Button();
        _closeButton.Text = "X";
        _closeButton.Size = new Vector2(48, _closeButton.Size.Y);
        _closeButton.Pressed += RemoveTool;
        horizontalContainer.AddChild(_closeButton);
        
        return headerContainer;
    }

    protected abstract void BuildPanelWidgets();

    #endregion
}