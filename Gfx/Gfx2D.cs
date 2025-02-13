using AssistLib.DB.Runtime;
using c1tr00z.AssistLib.Common;
using Godot;

namespace projectwitch.addons.AssistLib.Gfx;

[GlobalClass]
public partial class Gfx2D : Node2D, IGfx {

    #region Private Fields

    private GfxBaseDBEntry _dbEntry;

    private GpuParticles2D _particles;

    #endregion

    #region Export Fields

    [Export] private NodePath _particlesPath = "GPUParticles2D";

    #endregion

    #region Accessors

    private GpuParticles2D particles => this.GetCachedInChildren(ref _particles, _particlesPath);

    #endregion

    #region IGfx Implementation

    public GfxBaseDBEntry baseDBEntry => this.GetCachedNodeDBEntry(ref _dbEntry);

    #endregion
    
    #region Node Implementation

    public override void _EnterTree() {
        base._EnterTree();
        particles.Finished += OnFinished;
    }

    public override void _ExitTree() {
        base._ExitTree();
        particles.Finished -= OnFinished;
    }

    #endregion

    #region Class Implementation
    
    private void OnFinished() {
        this.ReturnToPool();
    }

    public void Play() {
        particles.Restart();
    }

    #endregion
}