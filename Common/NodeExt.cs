using System;
using System.Linq;
using Godot;

namespace c1tr00z.AssistLib.Common;

public static class NodeExt {
    #region Class Implementation

    public static T GetCachedInChildren<T>(this Node node, ref T cached) where T : Node {
        if (cached == null) {
            cached = FindInChildrenByType<T>(node);
        }
        
        return cached;
    }

    public static T GetCachedInParents<T>(this Node node, ref T cached) where T : Node {
        if (cached == null) {
            cached = FindInParentsByType<T>(node);
        }

        return cached;
    }

    public static T GetCached<T>(ref T cached, Func<T> getter) {
        if (cached == null) {
            cached = getter.Invoke();
        }

        return cached;
    }

    public static T FindInChildrenByType<T>(this Node root) {
        if (root is T tObj) {
            return tObj;
        }
        var allChildren = root.GetChildren(true).ToList();
        var demanded = allChildren.OfType<T>().FirstOrDefault();
        if (demanded == null) {
            foreach (var child in allChildren) {
                demanded = FindInChildrenByType<T>(child);
                if (demanded != null) {
                    return demanded;
                }
            }
        }

        return demanded;
    }

    public static T FindInParentsByType<T>(this Node child) {
        if (child is T tChild) {
            return tChild;
        }
        var parent = child.GetParent();
        if (parent == null) {
            return default;
        }

        if (parent is T result) {
            return result;
        }

        return FindInParentsByType<T>(parent);
    }

    #endregion
}