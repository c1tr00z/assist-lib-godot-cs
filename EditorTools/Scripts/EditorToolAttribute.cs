using System;

namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

public class EditorToolAttribute(string toolTitle) : Attribute {
    #region Accessors

    public string toolTitle { get; } = toolTitle;

    #endregion
}