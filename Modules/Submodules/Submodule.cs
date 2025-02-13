using Godot;

namespace c1tr00z.AssistLib.Modules.Submodules;

public abstract partial class Submodule : Node { }

public abstract partial class SubmoduleGeneric<T> : Submodule where T : Module {

    #region Private Fields

    private T _module = null;

    #endregion

    #region Accessors

    protected T module => ModulesExt.GetCached(ref _module);

    #endregion

}