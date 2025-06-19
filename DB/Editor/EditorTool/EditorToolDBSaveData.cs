using c1tr00z.AssistLib.Json;
using projectwitch.addons.AssistLib.EditorTools.Scripts;

namespace AssistLib.DB.Editor.EditorTool;

public class EditorToolDBSaveData : EditorToolData {
    #region Public Fields
    [JsonSerializableField] public bool forceReloadProject;
    #endregion
}