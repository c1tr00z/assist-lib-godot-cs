using System;
using System.Threading.Tasks;

namespace c1tr00z.AssistLib.Modules;

public abstract partial class SceneModule : Module {

    #region Events

    public static event Action<SceneModule, InitResult> Initialized;

    #endregion

    #region Nested Classes

    public enum InitResult {
        Success,
        Fail,
    }

    #endregion

    #region Class Implementation

    public async Task<InitResult> Init() {
        var result = await InitModule();
        Initialized?.Invoke(this, result);
        return result;
    }

    protected virtual async Task<InitResult> InitModule() {
        await Task.Delay(100);
        return InitResult.Success;
    }

    #endregion

}