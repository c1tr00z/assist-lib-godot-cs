using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using c1tr00z.AssistLib.Common;
using c1tr00z.AssistLib.Json;
using Godot;
using projectwitch.addons.AssistLib.EditorSettings;

namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

[Tool]
public class EditorToolsController {

    #region Events

    public static event Action RequestData;

    public static event Action<AssistLibEditorTool> ToolAdded;
    
    public static event Action<AssistLibEditorTool> ToolRemoved; 

    #endregion
    
    #region Private Fields
    
    private static string SAVE_KEY = "AssistLib.EditorTools.Data";

    private static EditorToolsController _instance;

    private EditorToolsData _toolsData;
    
    private List<Type> _toolsTypes = new();

    private Dictionary<String, Type> _allToolsTypes = new();

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

    public Dictionary<String, Type> allToolsTypes {
        get {
            if (_allToolsTypes.Count == 0) {
                _allToolsTypes = ReflectionUtils.GetSubclassesOf<AssistLibEditorTool>(false).ToDictionary(t => {
                    var attribute = t.GetCustomAttributes<EditorToolAttribute>().FirstOrDefault();
                    return attribute is null ? t.Name : attribute.toolTitle;
                }, t => t);
            }

            return _allToolsTypes;
        }
    }

    #endregion

    #region Class Implementation

    private void Init() {
        if (_toolsTypes.Count == 0) {
            _toolsTypes = ReflectionUtils.GetSubclassesOf<AssistLibEditorTool>(false);
        }
        var jsonString = AssistLibEditorSettings.Get<string>(SAVE_KEY);
        if (jsonString.IsNullOrEmpty()) {
            _toolsData = new EditorToolsData();
        } else {
            _toolsData = JSONUtils.FromJsonString<EditorToolsData>(jsonString);
        }

        var allToolsTypesList = allToolsTypes.Values.ToList();
        _toolsData.toolsData.ForEach(d => {
            var dataType = d.GetType();
            var toolType = allToolsTypesList.FirstOrDefault(t => t.GetGenericArguments().Contains(dataType) || t.BaseType.GetGenericArguments().Contains(dataType));
            AddTool(toolType);
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

    public void AddTool(Type toolType) {
        if (!typeof(AssistLibEditorTool).IsAssignableFrom(toolType)) {
            throw new Exception($"toolType ({toolType.FullName} has to be assignable from AssistLibEditorTool");
        }
        
        var tool = Activator.CreateInstance(toolType) as AssistLibEditorTool;
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
            ToolAdded?.Invoke(tool);
        }
    }

    public void Remove(AssistLibEditorTool tool) {
        
        if (tools.Contains(tool)) {
            tools.Remove(tool);
        }
        
        ToolRemoved?.Invoke(tool);
        
        SaveTools();
    }

    #endregion
}