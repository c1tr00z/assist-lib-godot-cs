using System;
using System.Linq;

namespace c1tr00z.AssistLib.UI;

public partial class UIViewItem<T> : UIViewBase {

    #region Accessors

    protected T item { get; private set; }

    #endregion
    
    #region UIViewBase Implementation

    protected override void OnArgs(params object[] args) {
        item = args.OfType<T>().FirstOrDefault();
    }

    #endregion

    #region Class Implementation

    protected virtual void UpdateView() { }

    #endregion
}