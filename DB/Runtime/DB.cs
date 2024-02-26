using System.Collections.Generic;
using System.Linq;
using c1tr00z.AssistLib.Common;
using Godot;

namespace AssistLib.DB.Runtime;

public static class DB {

    #region Private Fields

    private static Dictionary<DBEntry, DBEntryData> _entries = new();

    private static List<DBEntry> _entriesList = new();
    
    #endregion

    #region Class Implementation

    private static Dictionary<DBEntry, DBEntryData> GetAllDic() {
        if (_entries.Count == 0) {
            var collection = GD.Load<DBCollection>("res://db/db.tres");
            _entries = collection.dbEntryDatas.ToDictionary(d => GD.Load<DBEntry>($"{d.dbEntryPath}.tres"), d => d);
        }

        return _entries;
    }
    
    private static List<DBEntry> GetAllList() {
        if (_entriesList.Count == 0) {
            _entriesList = GetAllDic().Keys.ToList();
        }

        return _entriesList;
    }

    public static string GetDBEntryName(DBEntry dbEntry) {
        if (!_entries.TryGetValue(dbEntry, out DBEntryData data)) {
            return string.Empty;
        }

        return data.dbEntryName;
    }
    
    public static string GetDBEntryPath(DBEntry dbEntry) {
        if (!_entries.TryGetValue(dbEntry, out DBEntryData data)) {
            return string.Empty;
        }

        return data.dbEntryPath;
    }

    public static List<T> GetAll<T>() where T : DBEntry {
        return GetAllList().OfType<T>().ToList();
    }

    public static T Get<T>(string dbEntryName = null) where T : DBEntry {
        var all = GetAll<T>();
        if (dbEntryName.IsNullOrEmpty()) {
            return all.FirstOrDefault();
        }
        return all.FirstOrDefault(dbEntry => GetDBEntryName(dbEntry) == dbEntryName);
    }

    #endregion
}