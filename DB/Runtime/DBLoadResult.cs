using Godot;

namespace AssistLib.DB.Runtime;

public enum LoadResult {
    Success,
    Failed,
}

public class DBLoadResult<T> where T : Resource {
    #region Public Fields

    public LoadResult result;

    public string error;

    public T resource;

    #endregion
}