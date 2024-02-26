using System.Collections.Generic;
using System.Linq;
using Godot;

namespace c1tr00z.AssistLib.Modules;

public static class Modules {

    #region Private Fields

    private static List<SceneModules> _sceneModules = new();

    private static List<Module> _rootModules = new();

    #endregion
    
    #region Class Implementation

    public static void AddRootModule(Module module) {
        _rootModules.Add(module);
    }

    public static void AddSceneModules(SceneModules sceneModules) {
        if (_sceneModules.Contains(sceneModules)) {
            return;
        }
        
        _sceneModules.Add(sceneModules);
    }

    public static void RemoveSceneModules(SceneModules sceneModules) {
        if (!_sceneModules.Contains(sceneModules)) {
            return;
        }

        _sceneModules.Remove(sceneModules);
    }

    public static T Get<T>() where T : Module {
        var type = typeof(T);
        if (typeof(SceneModule).IsAssignableFrom(type)) {
            return GetSceneModule<T>();
        }

        return _rootModules.OfType<T>().FirstOrDefault();
    }

    private static T GetSceneModule<T>() where T : Module {
        foreach (var m in _sceneModules) {
            if (m.TryGet(out T module)) {
                return module;
            }
        }

        return null;
    }

    #endregion
}