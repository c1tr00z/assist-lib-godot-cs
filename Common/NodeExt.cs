using System;
using System.Collections.Generic;
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
    
    public static T GetCached<T>(this Node parent, ref T cached, NodePath nodePath) where T : Node {
        return cached ??= parent.GetNodeOrNull<T>(nodePath);
    }

    public static bool TryGetCached<T>(this Node parent, ref T cached, NodePath nodePath) where T : Node {
        cached ??= parent.GetNodeOrNull<T>(nodePath);

        return cached != null;
    }

    public static List<T> FindAllInChildrenByType<T>(this Node root, bool recursively = false) {
        var result = new List<T>();

        var allChildren = root.GetChildren(true).ToList();
        result.AddRange(allChildren.OfType<T>());

        if (recursively) {
            allChildren.ForEach(c => result.AddRange(c.FindAllInChildrenByType<T>(true)));
        }
        
        return result;
    }

    public static T FindInChildrenByType<T>(this Node root, bool recursively = false) {
        if (root is T tObj) {
            return tObj;
        }
        var allChildren = root.GetChildren(true).ToList();
        var demanded = allChildren.OfType<T>().FirstOrDefault();
        if (demanded == null && recursively) {
            foreach (var child in allChildren) {
                demanded = FindInChildrenByType<T>(child, true);
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
    
    public static bool TryFindInParentsByType<T>(this Node child, out T outResult) {
        outResult = child.FindInParentsByType<T>();

        return outResult != null;
    }

    public static void EnableNode(this Node node) {
        node.CallDeferred(GodotObject.MethodName.Set, "process_mode", (long)Node.ProcessModeEnum.Inherit);

        if (node is Node2D node2D) {
            node2D.Show();
        }

        if (node is CanvasItem canvasItem) {
            canvasItem.Visible = true;
        }
    }

    public static void DisableNode(this Node node) {
        if (node is CanvasItem canvasItem) {
            canvasItem.Visible = false;
        }
        
        if (node is Node2D node2D) {
            node2D.Hide();
        }
        node.CallDeferred(GodotObject.MethodName.Set, "process_mode", (long)Node.ProcessModeEnum.Disabled);
    }

    #endregion
}