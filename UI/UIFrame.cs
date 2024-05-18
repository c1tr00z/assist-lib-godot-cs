using System.Collections.Generic;
using AssistLib.DB.Runtime;
using c1tr00z.AssistLib.Common;
using Godot;

namespace c1tr00z.AssistLib.UI;

[GlobalClass]
public partial class UIFrame : Control {

    #region Private Fields

    private List<UIViewBase> _views = new();

    private UIFrameDBEntry _dbEntry;

    #endregion

    #region Accessors

    private List<UIViewBase> views {
        get {
            if (_views.Count == 0) {
                _views = this.FindAllInChildrenByType<UIViewBase>();
            }

            return _views;
        }
    }

    public UIFrameDBEntry dbEntry => CommonExt.GetCached(ref _dbEntry,
        () => this.FindInChildrenByType<DBEntryResource>().dbEntry as UIFrameDBEntry);

    #endregion
    
    #region Class Implementation

    public void ShowFrame(params object[] args) {
        views.ForEach(v => v.ShowView(args));
    }

    public void HideFrame() {
        views.ForEach(v => v.HideView());
    }

    #endregion
}