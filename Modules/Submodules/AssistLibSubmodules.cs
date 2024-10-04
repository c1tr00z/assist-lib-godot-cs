namespace c1tr00z.AssistLib.Modules.Submodules;

public static class AssistLibSubmodules {

    #region Accessors

    private static AssistLibSubmodulesModule _submodules;

    #endregion

    #region Class Implementation

    public static void Init(AssistLibSubmodulesModule submodules) {
        _submodules = submodules;
    }

    public static T Get<T>() where T : Submodule {
        return _submodules.GetSubmodule<T>();
    }

    #endregion
}