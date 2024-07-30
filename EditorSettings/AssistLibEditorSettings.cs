using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssistLib.DB.Runtime;
using c1tr00z.AssistLib.Common;
using c1tr00z.AssistLib.Json;
using c1tr00z.AssistLib.Utils;
using Godot;
namespace projectwitch.addons.AssistLib.EditorSettings;

public class AssistLibEditorSettings {
    
    #region Private Fields

    private const string SETTINGS_PATH = "res://assistLibEditorSettings/assistLibEditorSettings.tres";
    
    private const string SETTINGS_DIR_PATH = "assistLibEditorSettings";
    
    private static AssistLibEditorSettings _instance;

    private AssistLibEditorSettingsResource _settingsResource;

    #endregion

    #region Accessors

    private static AssistLibEditorSettings instance {
        get {
            if (_instance == null) {
                _instance = new AssistLibEditorSettings();
                _instance.LoadSettings();
            }

            return _instance;
        }
    }

    #endregion

    #region Class Implementation

    private void LoadSettings() {
        if (_settingsResource != null) {
            return;
        }
        var directoryPath = ProjectSettings.Singleton.GlobalizePath("res://");
        var settingsDirectoryPath = Path.Join(directoryPath, SETTINGS_DIR_PATH);
        var settingsDirectory = new DirectoryInfo(settingsDirectoryPath);
        if (!settingsDirectory.Exists) {
            settingsDirectory.Create();
        }

        if (!ResourceLoader.Exists(SETTINGS_PATH)) {
            _settingsResource = new AssistLibEditorSettingsResource();
            Save();
        }
        _settingsResource = GD.Load<AssistLibEditorSettingsResource>(SETTINGS_PATH);
    }

    private void Save() {
        ResourceSaver.Singleton.Save(_settingsResource, SETTINGS_PATH);
    }

    private T GetByKey<T>(string key) {
        if (!_settingsResource.data.TryGetValue(key, out Variant value)) {
            return default;
        }

        var targetType = typeof(T);
        if (targetType.GetInterfaces().Contains(typeof(IJsonDeserializable))) {
            var stringValue = value.ToString();
            var json = JSONUtils.Deserialize(stringValue);
            var objValue = Activator.CreateInstance<T>() as IJsonDeserializable;
            objValue.Deserialize(json);
            return (T)objValue;
        }

        return (T)(object)value;
    }

    private void SetByKey(string key, object value) {
        var save = false;
        if (value is IJsonSerializable jsonSerializable) {
            var jsonString = jsonSerializable.ToJsonString();
            _settingsResource.data[key] = jsonString;
            save = true;
        } else if (value is int intValue) {
            _settingsResource.data[key] = intValue;
            save = true;
        } else if (value is double doubleValue) {
            _settingsResource.data[key] = doubleValue;
            save = true;
        } else if (value is float floatValue) {
            _settingsResource.data[key] = floatValue;
            save = true;
        } else if (value is string stringValue) {
            _settingsResource.data[key] = stringValue;
            save = true;
        } else if (value is Vector2 vector2Value) {
            _settingsResource.data[key] = vector2Value;
            save = true;
        } else if (value is Vector3 vector3Value) {
            _settingsResource.data[key] = vector3Value;
            save = true;
        } else if (value is Vector4 vector4Value) {
            _settingsResource.data[key] = vector4Value;
            save = true;
        } else {
            if (value is Variant variantValue) {
                _settingsResource.data[key] = variantValue;
                save = true;
            } else {
                GD.PushError($"Value {value} cant be set because its not Variant type");
            }
        }

        if (save) {
            Save();
        }
    }

    public static T Get<T>(string key) {
        return instance.GetByKey<T>(key);
    }

    public static void Set(string key, object value) {
        instance.SetByKey(key, value);
    }

    #endregion
}