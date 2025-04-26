using Godot;

namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

public static class EditorToolsPanelsConstants {
    #region Public Fields

    public static StringName PANEL_HEADER_STYLE_NAME = "panel";
    public static StyleBox PANEL_HEADER_STYLE = new StyleBoxFlat {
        BgColor = new Color("141414FF"),
        ContentMarginLeft = 10,
        ContentMarginRight = 10,
        ExpandMarginTop = 10,
        ExpandMarginBottom = 10,
    };

    #endregion
}
