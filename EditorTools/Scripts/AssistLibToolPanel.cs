using c1tr00z.AssistLib.Common;
using Godot;

namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

public abstract partial class AssistLibToolPanel<T> : Control where T : AssistLibEditorTool {

    #region Private Fields

    private T _tool;

    #endregion
    
    #region Accessors

    protected T tool => CommonExt.GetCached(ref _tool, EditorToolsController.Get<T>);

    #endregion
}