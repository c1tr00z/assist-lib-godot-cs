using System;
using System.Collections.Generic;
using System.Linq;
using c1tr00z.AssistLib.Common;
using Godot;
using Array = System.Array;

namespace c1tr00z.AssistLib.UI;

public abstract partial class UIViewBase : Node2D {

    #region Events

    public event Action Updated; 

    #endregion

    #region Private Fields

    private List<UIViewBase> _parentViews = new();

    private object[] _args = Array.Empty<object>();
    
    private List<UIViewBase> _childViews = new ();
    
    #endregion

    #region Export Fields

    [Export] private Godot.Collections.Array<NodePath> _parentViewsPaths = new();

    #endregion

    #region Accessors

    public bool isShowing { get; private set; }

    public object[] viewArgs {
        get {
            var argsList = new List<object>();

            argsList.AddRange(_args);
            argsList.AddRange(GetViewParams());
            
            return argsList.ToArray();
        }
    }

    public virtual Dictionary<StringName, object> namedParameters {
        get {
            var parameters = new Dictionary<StringName, object>();
            _parentViews.ForEach(v => {
                var parentNamedParameters = v.namedParameters;
                foreach (var key in parentNamedParameters.Keys) {
                    parameters[key] = parentNamedParameters[key];
                }
            });
            var myNamedParams = GetViewNamedParams();
            foreach (var key in myNamedParams.Keys) {
                parameters[key] = myNamedParams[key];
            }
            return parameters;
        }
    }

    #endregion
    
    #region Class Implementation

    public void ShowView(params object[] args) {
        _args = args;
        if (!isShowing) {
            isShowing = true;
            _parentViews = _parentViewsPaths.SelectNotNull(p => GetNode(p) as UIViewBase).ToList();
            _parentViews.ForEach(v => v.Updated += OnParentViewUpdated);
            SubscribeToEvents();
        }
        
        OnArgs(args);
        
        UpdateChildViews();
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
    
    public virtual void OnMouseEntered() { }
    
    public virtual void OnMouseExited() { }
    
    protected virtual object[] GetViewParams() {
        return Array.Empty<object>();
    }

    protected virtual void UpdateView() { }
    
    protected virtual Dictionary<StringName, object> GetViewNamedParams() {
        return new Dictionary<StringName, object>();
    }

    protected void OnUpdated() {
        Updated?.Invoke();
    }

    protected void UpdateChildViews() {
        if (_childViews.Count == 0) {
            _childViews = this.FindAllInChildrenByType<UIViewBase>();
        }
        _childViews.ForEach(v => v.ShowView(GetViewParams()));
    }

    #endregion
}