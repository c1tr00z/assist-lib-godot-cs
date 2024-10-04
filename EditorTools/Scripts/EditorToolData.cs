using System.Collections.Generic;
using c1tr00z.AssistLib.Json;

namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

public class EditorToolData : IEditorToolData, IJsonSerializableCustom {
    
    #region IJsonSerializableCustom

    public void SerializeCustom(Dictionary<string, object> json) {
        json["type"] = GetType().FullName;
    }

    #endregion
}