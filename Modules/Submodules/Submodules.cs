namespace c1tr00z.AssistLib.Modules.Submodules;

public static class Submodules {

    #region Accessors

    private static SubmodulesModule _submodules;

    #endregion

    #region Class Implementation

    public static void Init(SubmodulesModule submodules) {
        _submodules = submodules;
    }

    public static T Get<T>() where T : Submodule {
        return _submodules.GetSubmodule<T>();
    }

    #endregion
}