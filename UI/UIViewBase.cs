using System;
using System.Collections.Generic;
using System.Linq;
using c1tr00z.AssistLib.Common;
using Godot;
using Godot.Collections;

namespace c1tr00z.AssistLib.UI;

[GlobalClass]
public abstract partial class UIViewBase : Node2D {

    #region Events

    public event Action Updated; 

    #endregion

    #region Private Fields

    private List<UIViewBase> _parentViews = new();
    
    #endregion

    #region Export Fields

    [Export] private Array<NodePath> _parentViewsPaths = new();

    #endregion

    #region Accessors

    public bool isShowing { get; private set; }

    #endregion
    
    #region Class Implementation

    public void ShowView(params object[] args) {
        if (!isShowing) {
            isShowing = true;
            _parentViews = _parentViewsPaths.SelectNotNull(p => GetNode(p) as UIViewBase).ToList();
            _parentViews.ForEach(v => v.Updated += OnParentViewUpdated);
            SubscribeToEvents();
        }
        
        OnArgs(args);
    }

    protected abstract void OnArgs(params object[] args);

    public void HideView() {
        if (isShowing) {
            isShowing = false;
            _parentViews.ForEach(v => v.Updated -= OnParentViewUpdated);
            UnsubscribeFromEvents();
        }

        OnHide();
    }

    protected virtual void SubscribeToEvents() { }
    
    protected virtual void UnsubscribeFromEvents() { }

    protected virtual void OnHide() {}

    protected virtual void OnParentViewUpdated() { }

    protected void OnUpdated() {
        Updated?.Invoke();
    }

    #endregion
}