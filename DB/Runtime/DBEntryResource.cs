using Godot;

namespace AssistLib.DB.Runtime;

[GlobalClass]
public partial class DBEntryResource : Node {
    
    #region Export Fields

    [Export] public DBEntry dbEntry;
    [Export] public string key = "scene";

    #endregion
}