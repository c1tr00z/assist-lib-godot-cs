using System;

namespace c1tr00z.AssistLib.Modules;

public static class ModulesExt {
    
    #region Class Implementation

    public static T GetCached<T>(ref T cached) where T : Module {
        try {
            if (cached == null || cached.ToString() == "null") {
                cached = Modules.Get<T>();
            }
        } catch (ObjectDisposedException e) {
            cached = Modules.Get<T>();
        }
        
        return cached;
    }

    #endregion
}