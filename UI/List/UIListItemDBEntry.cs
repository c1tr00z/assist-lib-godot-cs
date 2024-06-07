using System;
using AssistLib.DB.Runtime;
using Godot;

namespace c1tr00z.AssistLib.UI.List;

[Tool]
[GlobalClass]
public partial class UIListItemDBEntry : DBEntry {
    
    #region Export Fields

    [Export] private String _fullTypeName;

    #endregion

    #region Accessors

    public String fullTypeName => _fullTypeName;

    #endregion
}