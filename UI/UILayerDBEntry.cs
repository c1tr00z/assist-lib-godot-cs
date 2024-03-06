using AssistLib.DB.Runtime;
using Godot;

namespace c1tr00z.AssistLib.UI;

[GlobalClass]
[Tool]
public partial class UILayerDBEntry : DBEntry {
    
    #region Export Fields

    [Export]
    public int index;

    #endregion
}