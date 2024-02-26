using System;
using Godot;

namespace AssistLib.DB.Runtime;

[Tool]
[GlobalClass]
public partial class DBEntryData : Resource {
    #region Exported Fields

    [Export] public String dbEntryName;
    [Export] public String dbEntryPath;

    #endregion
}