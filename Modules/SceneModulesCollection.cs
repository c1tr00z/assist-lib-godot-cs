using System;
using AssistLib.DB.Runtime;
using Godot;

namespace c1tr00z.AssistLib.Modules;

[Tool]
[GlobalClass]
public partial class SceneModulesCollection : DBEntry {
    #region Export Fields

    [Export]
    public SceneModuleDBEntry[] modulesDbEntries = Array.Empty<SceneModuleDBEntry>();

    #endregion
}