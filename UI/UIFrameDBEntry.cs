using AssistLib.DB.Runtime;
using Godot;

namespace c1tr00z.AssistLib.UI;

[Tool]
[GlobalClass]
public partial class UIFrameDBEntry : DBEntry {
    #region Export Fields

    [Export] public UILayerDBEntry layer;

    #endregion
}