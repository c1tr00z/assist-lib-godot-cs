using System;
using Godot;

namespace c1tr00z.AssistLib.Common;

public static class CommonExt {
    
    #region Class Implementation

    public static T GetCached<T>(ref T cached, Func<T> getter) {
        try {
            if (cached == null || cached.ToString() == "null") {
                cached = getter();
            }
        } catch (ObjectDisposedException e) {
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

    public static T GetCachedByPath<T>(ref T cached, Node node, NodePath path) where T : Node {
        if (cached == null) {
            cached = node.GetNode<T>(path);
        }

        return cached;
    }

    #endregion
}