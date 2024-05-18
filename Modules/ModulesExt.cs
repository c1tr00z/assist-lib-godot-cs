namespace c1tr00z.AssistLib.Modules;

public static class ModulesExt {
    #region Class Implementation

    public static T GetCached<T>(ref T cached) where T : Module {
        if (cached == null) {
            cached = Modules.Get<T>();
        }

        return cached;
    }

    #endregion
}