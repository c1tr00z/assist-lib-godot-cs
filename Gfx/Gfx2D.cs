using AssistLib.DB.Runtime;
using Godot;

namespace projectwitch.addons.AssistLib.Gfx;

[Tool]
public partial class Gfx2D : GpuParticles2D, IGfx {

    #region Private Fields

    private GfxBaseDBEntry _dbEntry;

    #endregion

    #region IGfx Implementation

    public GfxBaseDBEntry baseDBEntry => this.GetCachedNodeDBEntry(ref _dbEntry);

    #endregion
    
    #region Node Implementation

    public override void _EnterTree() {
        base._EnterTree();
        Finished += OnFinished;
    }

    #endregion

    #region Class Implementation
    
    private void OnFinished() {
        this.ReturnToPool();
    }

    #endregion
}