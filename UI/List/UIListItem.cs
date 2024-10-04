using System;
using System.Collections.Generic;
using AssistLib.DB.Runtime;
using c1tr00z.AssistLib.Common;
using Godot;

namespace c1tr00z.AssistLib.UI.List;

[GlobalClass]
public partial class UIListItem : Control {

    #region Events

    public event Action<UIListItem> Selected;

    #endregion
    
    #region Private Fields

    private List<UIViewBase> _views = new();

    private UIListItemDBEntry _dbEntry;

    private Type _itemType;

    #endregion

    #region Accessors
    
    public object item { get; private set; }

    private List<UIViewBase> views {
        get {
            if (_views.Count == 0) {
                _views = this.FindAllInChildrenByType<UIViewBase>();
            }

            return _views;
        }
    }
    
    public UIListItemDBEntry dbEntry => this.GetCachedNodeDBEntry(ref _dbEntry);

    public Type itemType =>
        CommonExt.GetCached(ref _itemType, () => ReflectionUtils.GetTypeByName(dbEntry.fullTypeName));

    #endregion

    #region Class Implementation

    public void SetItem(object item) {
        this.item = item;
        views.ForEach(v => v.ShowView(item));
    }

    public virtual void SubscribeToEvents() {
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
    }

    public virtual void UnsubscribeFromEvents() {
        MouseEntered -= OnMouseEntered;
        MouseExited -= OnMouseExited;
    }

    protected virtual void OnMouseEntered() {
        views.ForEach(v => v.OnMouseEntered());
    }

    protected virtual void OnMouseExited() {
        views.ForEach(v => v.OnMouseExited());
    }

    public void Select() {
        Selected?.Invoke(this);
    }

    #endregion
}