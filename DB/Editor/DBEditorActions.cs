#if TOOLS
using System.Collections.Generic;
using System.Linq;
using AssistLib.DB.Runtime;
using Godot;

namespace AssistLib.DB.Editor;

[Tool]
public static class DBEditorActions {
    
    #region Class Implementation

    public static void CollectDBEntries() {
        ScanFolderAndCollect("", out List<DBEntryData> entryData);
        
        entryData.ForEach(entryData => GD.Print($"Found: {entryData.dbEntryName} at {entryData.dbEntryPath}"));

        var db = GD.Load<DBCollection>("res://db/db.tres");
        db.dbEntryDatas = entryData.ToArray();
        ResourceSaver.Singleton.Save(db, db.ResourcePath);
    }

    private static void ScanFolderAndCollect(string pathToFolder, out List<DBEntryData> entryData) {
        var resPath = $"res://{pathToFolder}";
        var dir = DirAccess.Open(resPath);

        entryData = new List<DBEntryData>();
        foreach (var file in dir.GetFiles()) {
            var assetName = file.GetBaseName();

            if (file.GetExtension() == "tres") {
                var entryPath = $"res:/{pathToFolder}/{assetName}";
                var resourcePath = $"{entryPath}.{file.GetExtension()}";
                
                var asset = GD.Load(resourcePath);

                if (asset is DBEntry) {
                    var data = new DBEntryData();
                    data.dbEntryName = assetName;
                    data.dbEntryPath = entryPath;
                    entryData.Add(data);
                }
            }
        }
        
        var childDirectories = dir.GetDirectories();
        foreach (var d in childDirectories) {
            ScanFolderAndCollect($"{pathToFolder}/{d}", out List<DBEntryData> otherData);
            entryData.AddRange(otherData);
        }
    }

    #endregion
}
#endif  