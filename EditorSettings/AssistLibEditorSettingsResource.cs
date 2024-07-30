using Godot;
using Godot.Collections;

namespace projectwitch.addons.AssistLib.EditorSettings;

[Tool]
[GlobalClass]
public partial class AssistLibEditorSettingsResource : Resource {
    
    #region Export Fields

    [Export] public Dictionary<string, Variant> data = new();

    #endregion
}