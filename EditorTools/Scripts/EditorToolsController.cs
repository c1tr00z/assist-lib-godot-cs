using System;
using System.Collections.Generic;
using System.Linq;
using c1tr00z.AssistLib.Common;
using Godot;

namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

[Tool]
public class EditorToolsController {
    
    #region Private Fields

    private static EditorToolsController _instance;

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
        var allTypes = ReflectionUtils.GetSubclassesOf<AssistLibEditorTool>(false);
        allTypes.ForEach(t => {
            var tool = Activator.CreateInstance(t) as AssistLibEditorTool;
            if (tool == null) {
                return;
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

    #endregion
}