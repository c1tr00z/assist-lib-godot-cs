using System;
using System.Collections.Generic;
using System.Linq;
using c1tr00z.AssistLib.Common;
using c1tr00z.AssistLib.Json;
using Godot;
using projectwitch.addons.AssistLib.EditorSettings;

namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

[Tool]
public class EditorToolsController {

    #region Nested Class

    public class ToolsData : IJsonSerializable, IJsonDeserializable {
        [JsonSerializableField] public Dictionary<string, Dictionary<string, object>> jsonData = new();
    }

    #endregion
    
    #region Private Fields
    
    private static string SAVE_KEY = "AssistLib.EditorTools.Data";

    private static EditorToolsController _instance;

    private ToolsData _toolsData;

    #endregion

    #region Accessors

    public static EditorToolsController instance {
        get {
            if (_instance == null) {
                _instance = new EditorToolsController();
                _instance.Init();
            }

            return _instance;
        }
    }

    public List<AssistLibEditorTool> tools { get; } = new();

    #endregion

    #region Class Implementation

    private void Init() {
        var jsonString = AssistLibEditorSettings.Get<string>(SAVE_KEY);
        if (jsonString.IsNullOrEmpty()) {
            _toolsData = new ToolsData();
        } else {
            _toolsData = JSONUtils.FromJsonString<ToolsData>(jsonString);
        }
        var allTypes = ReflectionUtils.GetSubclassesOf<AssistLibEditorTool>(false);
        allTypes.ForEach(t => {
            var tool = Activator.CreateInstance(t) as AssistLibEditorTool;
            if (tool == null) {
                return;
            }

            if (_toolsData.jsonData.TryGetValue(tool.GetType().FullName, out Dictionary<string, object> toolJson)) {
                tool.LoadTool(toolJson);
            } else {
                tool.LoadTool(null);
            }
            tools.Add(tool);
        });
    }

    public T GetTool<T>() where T : AssistLibEditorTool {
        return tools.OfType<T>().FirstOrDefault();
    }

    public static T Get<T>() where T : AssistLibEditorTool {
        return instance.GetTool<T>();
    }

    public void SaveTools() {
        _toolsData.jsonData.Clear();
        tools.ForEach(t => {
            var json = new Dictionary<string, object>();
            t.SaveTool(json);
            _toolsData.jsonData.Add(t.GetType().FullName, json);
        });
        var jsonString = _toolsData.ToJsonString();
        AssistLibEditorSettings.Set(SAVE_KEY, jsonString);
    }

    #endregion
}