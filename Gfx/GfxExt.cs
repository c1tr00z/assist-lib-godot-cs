using c1tr00z.AssistLib.Modules;
using Godot;

namespace projectwitch.addons.AssistLib.Gfx;

public static class GfxExt {
    
    #region Private Fields

    private static GfxManager _gfxManager;

    #endregion

    #region Accessors

    private static GfxManager gfxManager => ModulesExt.GetCached(ref _gfxManager);

    #endregion
    
    #region Class Implementation

    public static void PlayAt(this Gfx2DDBEntry dbEntry, Vector2 position) {
        gfxManager.PlayAt2D(dbEntry, position);
    }

    public static void ReturnToPool(this IGfx gfx) {
        gfxManager.ReturnToPool(gfx);
    }

    #endregion
}