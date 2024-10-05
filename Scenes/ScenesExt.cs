using c1tr00z.AssistLib.Modules;
namespace c1tr00z.AssistLib.Scenes;

public static class ScenesExt {
    #region Class Implementation

    /// <summary>
    /// Loads provided scene
    /// </summary>
    /// <param name="scene"></param>
    public static void LoadThisScene(this SceneDBEntry scene) {
        Modules.Modules.Get<ScenesModule>().LoadScene(scene);
    }

    #endregion
}