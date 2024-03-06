namespace c1tr00z.AssistLib.UI;

public static class GUIExt {
    
    #region Class Implementation

    public static void Show(this UIFrameDBEntry uiFrame, params object[] args) {
        Modules.Modules.Get<GUI>().ShowFrame(uiFrame, args);
    }

    #endregion
}