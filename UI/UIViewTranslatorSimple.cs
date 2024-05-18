using c1tr00z.AssistLib.Common;
using Godot;

namespace c1tr00z.AssistLib.UI;

[GlobalClass]
public partial class UIViewTranslatorSimple : UIViewBase {
    
    #region UIViewBase Implementation

    protected override void OnArgs(params object[] args) {
        this.FindAllInChildrenByType<UIViewBase>().ForEach(v => v.ShowView(args));
    }

    protected override void OnParentViewUpdated() {
        base.OnParentViewUpdated();
        OnUpdated();
    }

    #endregion
}