using Godot;

namespace AssistLib.DB.Runtime;

public enum LoadResult {
    Loading,
    Success,
    Failed,
}

public class DBLoadResult<T> where T : Resource {
    
    #region Public Fields

    public LoadResult result = LoadResult.Loading;

    public string error;

    public T resource;

    public double progress;

    #endregion

    public DBLoadResult(DBEntry dbEntry, string key) {
        this.dbEntry = dbEntry;
        this.key = key;
    }

    #region Accessors

    public DBEntry dbEntry { get; }
    
    public string key { get; }

    public string assetName => $"{dbEntry.GetPath()}_{key}";

    #endregion
}