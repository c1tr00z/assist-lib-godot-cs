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
        AddChild(new HSeparator());
        AddChild(BuildHeaderPanel());
        AddChild(new HSeparator());
    }

    private Node BuildHeaderPanel() {
        var horizontalContainer = new HBoxContainer();
        
        var titleLabel = new Label();
        titleLabel.Text = toolTitle;
        titleLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        horizontalContainer.AddChild(titleLabel);

        _closeButton = new Button();
        _closeButton.Text = "X";
        _closeButton.Size = new Vector2(48, _closeButton.Size.Y);
        _closeButton.Pressed += RemoveTool;
        horizontalContainer.AddChild(_closeButton);
        
        return horizontalContainer;
    }

    protected abstract void BuildPanelWidgets();

    #endregion
}