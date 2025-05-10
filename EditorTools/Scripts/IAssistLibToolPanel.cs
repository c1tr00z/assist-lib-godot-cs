namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

public interface IAssistLibToolPanel {
    #region Accessors

    public AssistLibEditorTool tool { get; }

    public void Init(AssistLibEditorTool tool);

    #endregion
}