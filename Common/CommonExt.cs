using System;

namespace c1tr00z.AssistLib.Common;

public static class CommonExt {
    #region Class Implementation

    public static T GetCached<T>(ref T cached, Func<T> getter) {
        if (cached == null) {
            cached = getter();
        }

        return cached;
    }

    public static bool TryGetCached<T>(ref T cached, Func<T> getter) {
        if (cached == null) {
            cached = getter();
        }

        return cached != null;
    }

    #endregion
}