using System.Collections.Generic;
using System.Linq;
using c1tr00z.AssistLib.Common;
using c1tr00z.AssistLib.Json;

namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

public class EditorToolsData : IJsonSerializableCustom, IJsonDeserializableCustom {

    #region Public Fields

    public List<IEditorToolData> toolsData = new();

    #endregion

    #region IJsonSerializableCustom Implementation
        
    public void SerializeCustom(Dictionary<string, object> json) {
        var list = new List<Dictionary<string, object>>();
        toolsData.ForEach(t => {
            var toolJson = t.ToJson();
            toolJson["type"] = t.GetType().FullName;
            list.Add(t.ToJson());
        });
        json.Add("tools", list);
    }
        
    #endregion

    #region IJsonDeserializableCustom Implementation

    public void DeserializeCustom(Dictionary<string, object> json) {
        toolsData.Clear();
        if (!json.ContainsKey("tools")) {
            return;
        }

        var toolsJsonsList = json.Get<object>("tools") as List<object>;

        if (toolsJsonsList == null) {
            return;
        }
            
        toolsJsonsList.OfType<Dictionary<string, object>>().ToList().ForEach(toolJson => {
            if (!toolJson.TryGetValue("type", out object toolDataNameObj)) {
                return;
            }
            var type = ReflectionUtils.GetTypeByName(toolDataNameObj.ToString());
            if (type == null) {
                return;
            }
            var toolData = (IEditorToolData)JSONUtils.Deserialize(type, toolJson);
            toolsData.Add(toolData);
        });
    }

    #endregion
}