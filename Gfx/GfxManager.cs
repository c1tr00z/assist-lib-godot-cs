using System.Collections.Generic;
using System.Threading.Tasks;
using AssistLib.DB.Runtime;
using c1tr00z.AssistLib.Common;
using c1tr00z.AssistLib.Modules;
using Godot;

namespace projectwitch.addons.AssistLib.Gfx;

[GlobalClass]
public partial class GfxManager : SceneModule {

    #region Nested Classes
    
    private interface IGfxRequest {
        public GfxBaseDBEntry dbEntry { get; set; }
    }

    private struct GfxRequest2D : IGfxRequest {
        
        public GfxBaseDBEntry dbEntry { get; set; }
        
        public Vector2 position;
    }

    #endregion
    
    #region Private Fields

    private Dictionary<GfxBaseDBEntry, Queue<IGfx>> _cached = new();

    private Queue<IGfxRequest> _queue = new();

    private bool _inProcess = false;
    
    private Node _currentScene = null;

    #endregion

    #region Accessors

    private Node currentScene => CommonExt.GetCached(ref _currentScene, () => GetTree().CurrentScene);

    #endregion

    #region Class Implementation

    public void PlayAt2D(Gfx2DDBEntry dbEntry, Vector2 position) {
        _queue.Enqueue(new GfxRequest2D {
            dbEntry = dbEntry,
            position = position,
        });

        if (_inProcess) {
            return;
        }

        LoadAndPlay();
    }

    private async Task LoadAndPlay() {
        _inProcess = true;
        while (_queue.Count > 0) {
            var request = _queue.Dequeue();
            var gfx = await GetGfx(request.dbEntry);
            if (request is GfxRequest2D request2D) {
                (gfx as Node2D).GlobalPosition = request2D.position;
                (gfx as Gfx2D).Play();
            }
        }

        _inProcess = false;
    }

    private async Task<IGfx> GetGfx(GfxBaseDBEntry dbEntry) {
        if (_cached.ContainsKey(dbEntry) && _cached[dbEntry].Count > 0) {
            var cached = _cached[dbEntry].Dequeue();
            if (cached is Node cachedNode) {
                cachedNode.EnableNode();
            }
            return cached;
        }

        var gfxResult = await dbEntry.LoadSceneAsync();

        if (gfxResult.result != LoadResult.Success) {
            GD.PushError($"Error: {gfxResult.error}");
            return null;
        }

        var gfx = gfxResult.resource.Instantiate<IGfx>();
        if (gfx is Node node) {
            currentScene.CallDeferred("add_child", node);
        }

        return gfx;
    }

    public void ReturnToPool(IGfx gfx) {
        if (gfx is Node node) {
            node.DisableNode();
        }

        if (!_cached.ContainsKey(gfx.baseDBEntry)) {
            _cached[gfx.baseDBEntry] = new Queue<IGfx>();
        }
        _cached[gfx.baseDBEntry].Enqueue(gfx);
    }

    #endregion
}