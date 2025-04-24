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

    #region Events

    public static event Action RequestData;

    public static event Action<AssistLibEditorTool> ToolRemoved; 

    #endregion
    
    #region Private Fields
    
    private static string SAVE_KEY = "AssistLib.EditorTools.Data";

    private static EditorToolsController _instance;

    private EditorToolsData _toolsData;

    private List<Type> _toolsSaveTypes = new();
    
    private List<Type> _toolsTypes = new();

    private List<AssistLibEditorTool> _tools = new();

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
        if (_toolsTypes.Count == 0) {
            _toolsTypes = ReflectionUtils.GetSubclassesOf<AssistLibEditorTool>(false);
            _toolsSaveTypes = ReflectionUtils.GetTypesByInterface<IEditorToolData>(false);
        }
        var jsonString = AssistLibEditorSettings.Get<string>(SAVE_KEY);
        if (jsonString.IsNullOrEmpty()) {
            _toolsData = new EditorToolsData();
        } else {
            _toolsData = JSONUtils.FromJsonString<EditorToolsData>(jsonString);
        }
        var allTypes = ReflectionUtils.GetSubclassesOf<AssistLibEditorTool>(false);
        allTypes.ForEach(t => {
            var tool = Activator.CreateInstance(t) as AssistLibEditorTool;
            if (tool == null) {
                return;
            }

            var toolDataType = tool.GetType().BaseType.GenericTypeArguments.FirstOrDefault();
            var toolSaveData = _toolsData.toolsData.FirstOrDefault(save => toolDataType == save.GetType());

            if (toolSaveData == null) {
                toolSaveData = Activator.CreateInstance(toolDataType) as IEditorToolData;
            }

            if (tool.LoadTool(toolSaveData)) {
                tools.Add(tool);
            }
        });
    }

    public T GetTool<T>() where T : AssistLibEditorTool {
        return tools.OfType<T>().FirstOrDefault();
    }

    public static T Get<T>() where T : AssistLibEditorTool {
        return instance.GetTool<T>();
    }

    public void SaveTools() {
        RequestData?.Invoke();
        _toolsData.toolsData.Clear();
        tools.ForEach(t => {
            _toolsData.toolsData.Add(t.GetSaveData());
        });
        var jsonString = _toolsData.ToJsonString();
        AssistLibEditorSettings.Set(SAVE_KEY, jsonString);
    }

    public void Remove(AssistLibEditorTool tool) {
        var toolDataType = tool.GetType().BaseType.GenericTypeArguments.FirstOrDefault();
        var toolSaveData = _toolsData.toolsData.FirstOrDefault(save => toolDataType == save.GetType());
        if (toolSaveData is not null) {
            _toolsData.toolsData.Remove(toolSaveData);
        }

        if (_tools.Contains(tool)) {
            _tools.Remove(tool);
        }
        
        ToolRemoved?.Invoke(tool);
    }

    #endregion
}